namespace Domain.Dashboard.Entities;

public class CurrentUser
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required bool IsAuthenticated { get; init; }
    
    public static CurrentUser Anonymous => new()
    {
        Id = Guid.Empty,
        Name = "Anonymous",
        Email = "",
        IsAuthenticated = false,
    };
}