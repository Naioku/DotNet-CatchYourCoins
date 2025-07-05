namespace Domain.IdentityEntities;

public class ResultSignIn
{
    public bool SignedIn { get; }
    public bool RequiresTwoFactor { get; }
    
    private ResultSignIn(
        bool succeeded,
        bool requiresTwoFactor = false)
    {
        SignedIn = succeeded;
        RequiresTwoFactor = requiresTwoFactor;
    }

    public static ResultSignIn Success() => new(true);
    public static ResultSignIn TwoFactorRequired() => new(false, requiresTwoFactor: true);
}