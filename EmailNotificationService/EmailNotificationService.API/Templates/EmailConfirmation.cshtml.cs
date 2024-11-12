using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmailNotificationService.API.Templates;

public class EmailConfirmation : PageModel
{
    public string FullName { get; private set; } = null!;
    public string ConfirmationLink { get; private set; } = null!;
}
