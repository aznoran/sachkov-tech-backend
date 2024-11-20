using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class FileName : ComparableValueObject
{
    public string Value { get; }

    private FileName(string value)
    {
        Value = value;
    }
    
    public static implicit operator string(FileName fileName) =>
        fileName.Value;

    public static Result<FileName, Error> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsInvalid("file name");

        return new FileName(fileName);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}