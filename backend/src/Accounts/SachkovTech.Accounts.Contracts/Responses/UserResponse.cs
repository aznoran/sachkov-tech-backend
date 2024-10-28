namespace SachkovTech.Accounts.Contracts.Responses;

public record UserResponse(
    Guid Id,
    string FirstName,
    string SecondName,
    StudentAccountResponse StudentAccount,
    SupportAccountResponse SupportAccount,
    RoleResponse[] Roles);

public record StudentAccountResponse(
    Guid Id,
    Guid UserId,
    SocialNetworkResponse[] SocialNetworks,
    DateTime DateStartedStudying);
    
public record SupportAccountResponse(
    Guid Id,
    Guid UserId,
    SocialNetworkResponse[] SocialNetworks,
    string AboutSelf);

// public record AdminAccountResponse(
//     Guid Id,
//     Guid UserId);
    
public record SocialNetworkResponse(
    string Name, 
    string Link);
    
public record RoleResponse(
    Guid Id, 
    string Name);