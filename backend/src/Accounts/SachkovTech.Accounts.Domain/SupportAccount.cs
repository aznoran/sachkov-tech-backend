namespace SachkovTech.Accounts.Domain;

public class SupportAccount
{
    public const string SUPPORT = "Support";

    private SupportAccount()
    {
        
    }

    public SupportAccount(
        User user, 
        string aboutSelf)
    {
        Id = Guid.NewGuid();
        User = user;
        AboutSelf = aboutSelf;
    }
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public string AboutSelf { get; set; }
}