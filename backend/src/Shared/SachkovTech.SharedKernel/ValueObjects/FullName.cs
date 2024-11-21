using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects;

public class FullName : ComparableValueObject
{
    public static FullName Empty = new FullName(null, null);
    
    private FullName(string? firstName, string? secondName)
    {
        FirstName = firstName;
        SecondName = secondName;
    }

    public string? FirstName { get; }
    public string? SecondName { get; }

    public static Result<FullName, Error> Create(string? firstName, string? secondName)
    {
        if (firstName?.Trim().Length == 0)
            return Errors.General.ValueIsInvalid("first name");

        if (secondName?.Trim().Length == 0)
            return Errors.General.ValueIsInvalid("second name");


        return new FullName(firstName, secondName);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FirstName;
        yield return SecondName;
    }
}