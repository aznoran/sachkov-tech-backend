using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;
using NotificationService.SharedKernel;

namespace NotificationService.Entities.ValueObjects;

public class Email : ComparableValueObject
{
    private const string REGEX = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    public string Value { get; }

    private Email(string email)
    {
        Value = email;
    }

    public static Result<Email, Error> Create(string email)
    {
        Regex regex = new Regex(REGEX);

        if (regex.IsMatch(email) == false)
            return Error.Validation($"Specified email address is invalid! : {email}");

        return new Email(email);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}