using System;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public static class TestFactoryUsers
{
    public static CurrentUser DefaultUser1Authenticated { get; } = DefaultUser1();
    public static CurrentUser DefaultUser1Anonymous { get; } = DefaultUser1(false);
    
    public static CurrentUser DefaultUser2Authenticated { get; } = DefaultUser2();
    public static CurrentUser DefaultUser2Anonymous { get; } = DefaultUser2(false);

    private static CurrentUser DefaultUser1(bool authenticated = true) => new()
    {
        Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
        Email = "test1@example.com",
        Name = "Test User 1",
        IsAuthenticated = authenticated
    };
    
    private static CurrentUser DefaultUser2(bool authenticated = true) => new()
    {
        Id = Guid.Parse("12345678-1234-1234-1234-123456789013"),
        Email = "test2@example.com",
        Name = "Test User 2",
        IsAuthenticated = authenticated
    };
}