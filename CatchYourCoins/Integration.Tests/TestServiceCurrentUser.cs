using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Integration;

public class TestServiceCurrentUser : IServiceCurrentUser
{
    public CurrentUser User => new()
    {
        Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
        Email = "test@example.com",
        Name = "Test User",
        IsAuthenticated = true
    };

}