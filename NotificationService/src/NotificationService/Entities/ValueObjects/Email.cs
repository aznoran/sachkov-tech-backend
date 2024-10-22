using CSharpFunctionalExtensions;
using NotificationService.HelperClasses;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Entities
{
    public record Email
    {
        public string value { get; }

        private Email(string email)
        {
            value = email;
        }

        public static Result<Email, Error> Create(string email)
        {
            var validator = new EmailAddressAttribute();

            if (validator.IsValid(email) == false || email is null)
                return Error.Validation($"Specified email address is invalid! : {email}");

            return new Email(email);
        }
    }
}
