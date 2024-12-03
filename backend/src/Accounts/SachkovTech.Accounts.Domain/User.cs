using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }

    public DateTime RegistrationDate { get; set; }

    public FullName FullName { get; set; }
    public Photo? Photo { get; set; }

    public IReadOnlyList<Role> Roles => _roles;
    private List<Role> _roles = [];

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    private List<SocialNetwork> _socialNetworks = [];

    public StudentAccount? StudentAccount;

    public SupportAccount? SupportAccount;

    public AdminAccount? AdminAccount;

    public static Result<User, Error> CreateAdmin(
        string userName,
        string email,
        FullName fullName,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            return Errors.User.InvalidRole();

        return new User
        {
            UserName = userName,
            Email = email,
            RegistrationDate = DateTime.UtcNow,
            FullName = fullName,
            _roles = [role],
            _socialNetworks = []
        };
    }

    public static Result<User, Error> CreateParticipant(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            return Errors.User.InvalidRole();

        return new User
        {
            UserName = userName,
            Email = email,
            RegistrationDate = DateTime.UtcNow,
            FullName = FullName.Empty,
            _roles = [role],
            _socialNetworks = []
        };
    }

    public void EnrollParticipant(Role role)
    {
        if (!_roles.Contains(role) && role.Name == StudentAccount.STUDENT)
            _roles.Add(role);
    }
}