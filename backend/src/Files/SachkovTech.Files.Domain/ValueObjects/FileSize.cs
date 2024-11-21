using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class FileSize : ComparableValueObject
{
    public long Value { get; }
    
    private FileSize(long value)
    {
        Value = value;
    }

    public static Result<FileSize, Error> Create(long fileLength)
    {
        if (fileLength <= 0)
            return Errors.General.ValueIsInvalid("file length");

        return new FileSize(fileLength);
    }

    public static implicit operator long(FileSize fileSize) =>
        fileSize.Value;
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}