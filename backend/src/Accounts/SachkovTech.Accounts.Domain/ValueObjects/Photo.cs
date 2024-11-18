using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Domain.ValueObjects;

public class Photo : ValueObject
{
    private static string[] PERMITED_FILES_TYPE =
            { "image/jpg", "image/jpeg", "image/png", "image/gif" };

    private static string[] PERMITED_EXTENSIONS =
            { "jpg", "jpeg", "png", "gif" };

    private static long MAX_FILE_SIZE = 5242880;

    private Photo(Guid fileId, string fileName, string contentType, long size)
    {
        FileId = fileId;
        FileName = fileName;
        ContentType = contentType;
        Size = size;
    }

    public Guid FileId { get; }
    public string FileName { get; } = default!;
    public string ContentType { get; } = default!;
    public long Size { get; }


    public static Result<Photo, Error> Create(
        Guid fileId, 
        string fileName, 
        string contentType, 
        long size)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Errors.General.ValueIsInvalid(fileName);
        }

        var fileExtension = fileName[fileName.LastIndexOf('.')..];

        if (!PERMITED_EXTENSIONS.Any(x => x == fileExtension))
        {
            return Errors.General.ValueIsInvalid(fileName);
        }

        if (!PERMITED_FILES_TYPE.Any(x => x == contentType))
        {
            return Errors.General.ValueIsInvalid(contentType);
        }

        if (size > MAX_FILE_SIZE)
        {
            return Errors.General.ValueIsInvalid($"File size > {MAX_FILE_SIZE}");
        }

        return new Photo(fileId, fileName, contentType, size);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FileId;
    }
}