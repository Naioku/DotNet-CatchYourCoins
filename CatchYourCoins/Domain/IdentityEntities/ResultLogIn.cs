namespace Domain.IdentityEntities;

public class ResultLogIn
{
    public bool SignedIn { get; }
    public bool RequiresTwoFactor { get; }
    
    private ResultLogIn(
        bool succeeded,
        bool requiresTwoFactor = false)
    {
        SignedIn = succeeded;
        RequiresTwoFactor = requiresTwoFactor;
    }

    public static ResultLogIn Success() => new(true);
    public static ResultLogIn TwoFactorRequired() => new(false, requiresTwoFactor: true);
}