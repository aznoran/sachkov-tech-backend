using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects;

public class Video : ComparableValueObject
{
    private static string[] PERMITED_FILES_TYPE =
            { "video/mp4", "video/mkv", "video/avi", "video/mov" };

    private static string[] PERMITED_EXTENSIONS =
            { "mp4", "mkv", "avi", "mov" };

    private static long MAX_FILE_SIZE = 4294967296;

    public Video(Guid fileId)
    {
        FileId = fileId;
    }

    public Guid FileId { get; }   

    public static UnitResult<Error> Validate(
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
            return Errors.Files.InvalidExtension();
        }

        if (!PERMITED_FILES_TYPE.Any(x => x == contentType))
        {
            return Errors.General.ValueIsInvalid(contentType);
        }

        if (size > MAX_FILE_SIZE)
        {
            return Errors.Files.InvalidSize();
        }

        return Result.Success<Error>();
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FileId;
    }
}