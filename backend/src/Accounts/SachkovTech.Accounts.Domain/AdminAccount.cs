using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Domain;

public class AdminAccount
{
    public const string ADMIN = "Admin";

    //ef core
    private AdminAccount()
    {
    }

    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }

    public Guid Id { get; set; }

    public User User { get; set; }
}