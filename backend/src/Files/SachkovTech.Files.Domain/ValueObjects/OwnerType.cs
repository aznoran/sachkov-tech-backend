using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Domain.ValueObjects;

public class OwnerType : ComparableValueObject
{
    public static readonly OwnerType Issue = new(nameof(Issue).ToUpper());

    private static readonly OwnerType[] _types = [Issue];

    public string Value { get; }

    private OwnerType(string value)
    {
        Value = value;
    }

    public static Result<OwnerType, Error> Create(string input)
    {
        var fileOwnerType = input.Trim().ToUpper();

        if (_types.Any(t => t.Value == fileOwnerType) == false)
        {
            return Errors.General.ValueIsInvalid("owner type");
        }

        return new OwnerType(fileOwnerType);
    }
    
    public static implicit operator string(OwnerType ownerType) =>
        ownerType.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}