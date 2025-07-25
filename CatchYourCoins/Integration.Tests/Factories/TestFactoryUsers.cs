namespace Integration.Factories;

public static class TestFactoryUsers
{
    public static User DefaultUser1() => new()
    {
        Name = "TestUser1",
        Email = "test1@example.com",
        Password = "Test.1234"
    };
    
    public static User DefaultUser2() => new()
    {
        Name = "TestUser2",
        Email = "test2@example.com",
        Password = "Test.1234"
    };
}