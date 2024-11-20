namespace EmailNotificationService.API.Requests;

public record MailConfirmationRequest(string Email, string FullName, string ConfirmationLink);