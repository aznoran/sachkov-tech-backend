using CSharpFunctionalExtensions;
using NotificationService.HelperClasses;
using System.Text.RegularExpressions;

namespace NotificationService.Entities
{
    public class Email : ValueObject
    {
        private const string REGEX = "^([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22))*\x40([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d))*$";
        public string value { get; }

        private Email(string email)
        {
            value = email;
        }

        public static Result<Email, Error> Create(string email)
        {
            Regex regex = new Regex(REGEX);

            if (regex.IsMatch(email) == false)
                return Error.Validation($"Specified email address is invalid! : {email}");

            return new Email(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return value;
        }

    }
}
