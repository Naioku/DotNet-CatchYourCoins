using System;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;
using Moq;

namespace Application.Tests.Factories;

public static class TestFactoryUsers
{
    public static CurrentUser DefaultUser1(bool authenticated = true) => new()
    {
        Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
        Email = "test1@example.com",
        Name = "Test User 1",
        IsAuthenticated = authenticated
    };
    
    public static CurrentUser DefaultUser2(bool authenticated = true) => new()
    {
        Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
        Email = "test2@example.com",
        Name = "Test User 2",
        IsAuthenticated = authenticated
    };

    public static Mock<IServiceCurrentUser> MockServiceCurrentUser(CurrentUser loggedInUser)
    {
        var mock = new Mock<IServiceCurrentUser>();
        mock.Setup(m => m.User).Returns(loggedInUser);
        return mock;
    }
}