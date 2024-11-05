using CSharpFunctionalExtensions;
using FileService.Application.Interfaces;
using FileService.Data.Dtos;
using FileService.Data.Models;
using FileService.Data.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System.Runtime.CompilerServices;

namespace FileService.Infrastrucure.Providers;
public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;
    private readonly MinioLimitations _limitations;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger, IOptions<MinioLimitations> limitations)
    {
        _minioClient = minioClient;
        _logger = logger;
        _limitations = limitations.Value;
    }

    public async IAsyncEnumerable<Result<UploadFilesResult, Error>> UploadFiles(
        IEnumerable<UploadFileData> filesData,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(_limitations.MaxDegreeOfParallelism);

        List<UploadFilesResult> results = [];

        var uploadFileDatas = filesData.ToList();

        var bucketsExistResult = await IfBucketsNotExistCreateBucket(
            uploadFileDatas.Select(file => file.FilePath.BucketName).Distinct(),
            cancellationToken);

        if (bucketsExistResult.IsFailure)
            yield break;

        var tasks = uploadFileDatas.Select(async file =>
            await PutObject(file, semaphoreSlim, cancellationToken)).ToList();

        while (tasks.Count != 0)
        {
            var task = await Task.WhenAny(tasks);

            var result = await task;

            tasks.Remove(task);

            if (result.IsSuccess)
                results.Add(result.Value);

            yield return result;
        }

        _logger.LogInformation("Uploaded files: {files}", results.Select(f => f.FilePath.FullPath));
    }

    public async Task<IEnumerable<GetLinkFileResult>> GetLinks(
            IEnumerable<FilePath> filePaths,
            CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(_limitations.MaxDegreeOfParallelism);

        var bucketsExistResult = await IfBucketsNotExistCreateBucket(
            filePaths.Select(file => file.BucketName).Distinct(),
            cancellationToken);

        if (bucketsExistResult.IsFailure)
            return [];

        var tasks = filePaths.Select(async file =>
                await GetLink(file, semaphoreSlim, cancellationToken)).ToList();

        await Task.WhenAll(tasks);

        var results = tasks.Select(t => t.Result).ToList();

        _logger.LogInformation("Received links to files: {links}", results.Select(r => r.FilePath));

        return results;
    }

    public async Task<UnitResult<Error>> RemoveFile(
        FilePath filePath,
        CancellationToken cancellationToken = default)
    {
        var bucketsExistResult = await IfBucketsNotExistCreateBucket([filePath.BucketName], cancellationToken);

        if (bucketsExistResult.IsFailure)
            return bucketsExistResult.Error;

        try
        {

            var statArgs = new StatObjectArgs()
                .WithBucket(filePath.BucketName)
                .WithObject(filePath.FileNameWithPrefix);

            var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat.ContentType == null)
                return Result.Success<Error>();

            var removeArgs = new RemoveObjectArgs()
                .WithBucket(filePath.BucketName)
                .WithObject(filePath.FileNameWithPrefix);

            await _minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to remove file in minio with path {path} in bucket {bucket}",
                filePath.FileNameWithPrefix,
                filePath.BucketName);

            return Errors.Files.FailRemove();
        }

        return Result.Success<Error>();
    }



    private async Task<Result<UploadFilesResult, Error>> PutObject(
        UploadFileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var fileNameGuid = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(fileData.FilePath.FileName);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FilePath.BucketName)
            .WithStreamData(fileData.ContentStream)
            .WithObjectSize(fileData.ContentStream.Length)
            .WithObject(fileData.FilePath.Prefix + "/" + fileNameGuid + fileExtension);

        try
        {
            var response = await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            _logger.LogInformation("Uploaded file with path {path}", response.ObjectName);

            var filePath = new FilePath(fileData.FilePath.BucketName + "/" + response.ObjectName);
            var fileSize = response.Size;

            var result = new UploadFilesResult(
                fileData.FilePath.BucketName,
                fileData.FilePath.FileName,
                filePath,
                fileSize);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.FileNameWithPrefix,
                fileData.FilePath.BucketName);

            return Errors.Files.FailUpload();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }


    private async Task<GetLinkFileResult> GetLink(
            FilePath filePath,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
    {
        var objectName = filePath.Prefix + "/" + filePath.FileName;

        await semaphoreSlim.WaitAsync(cancellationToken);


        var statArgs = new StatObjectArgs()
            .WithBucket(filePath.BucketName)
            .WithObject(objectName);

        var getLinkArgs = new PresignedGetObjectArgs()
            .WithBucket(filePath.BucketName)
            .WithObject(objectName)
            .WithExpiry(_limitations.LinkExpiry);

        try
        {
            var statFile = await _minioClient.StatObjectAsync(statArgs);

            if (statFile.ContentType == null)
            {
                _logger.LogError(
                    "The file with the bucket \"{BucketName}\" and the name \"{FileName}\" was not found",
                    filePath.BucketName,
                    objectName);

                return new GetLinkFileResult(filePath, "");
            }


            var getLinkResult = await _minioClient.PresignedGetObjectAsync(getLinkArgs);

            if (getLinkResult is null)
            {
                _logger.LogError(
                    "The file with the bucket \"{BucketName}\" and the name \"{FileName}\" was not found",
                    filePath.BucketName,
                    objectName);

                return new GetLinkFileResult(filePath, "");
            }


            return new GetLinkFileResult(filePath, getLinkResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to get link file from minio with path {path} in bucket {bucket}",
                filePath.FileNameWithPrefix,
                filePath.BucketName);

            return new GetLinkFileResult(filePath, "");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }


    private async Task<UnitResult<Error>> IfBucketsNotExistCreateBucket(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [.. buckets];

        foreach (var bucketName in bucketNames)
        {
            try
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist) continue;

                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking the existence of a bucket named : {bucketName}", bucketName);

                return Error.Failure("file.bucket.not.exists",
                    $"Error checking the existence of a bucket named : {bucketName}");
            }
        }

        return UnitResult.Success<Error>();
    }
}
