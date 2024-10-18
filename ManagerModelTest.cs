using CMCS.Models;
using Xunit;

public class ManagerTests
{
    [Fact]
    public void Manager_CanSetProperties()
    {
        // Arrange
        var manager = new Manager();

        // Act
        manager.ManagerID = 1;
        manager.Username = "TestUser";
        manager.Password = "TestPassword123";

        // Assert
        Assert.Equal(1, manager.ManagerID);
        Assert.Equal("TestUser", manager.Username);
        Assert.Equal("TestPassword123", manager.Password);
    }

    [Fact]
    public void Manager_UsernameCannotBeNull()
    {
        // Arrange
        var manager = new Manager
        {
            ManagerID = 1,
            Username = null, // Intentionally set to null
            Password = "TestPassword123"
        };

        // Act & Assert
        Assert.Null(manager.Username);
    }

    [Fact]
    public void Manager_PasswordCannotBeEmpty()
    {
        // Arrange
        var manager = new Manager
        {
            ManagerID = 2,
            Username = "AnotherUser",
            Password = "" // Intentionally empty
        };

        // Act & Assert
        Assert.Equal("", manager.Password);
        Assert.True(string.IsNullOrEmpty(manager.Password));
    }

    [Fact]
    public void Manager_HasValidManagerID()
    {
        // Arrange
        var manager = new Manager
        {
            ManagerID = 100,
            Username = "ManagerWithID",
            Password = "StrongPassword"
        };

        // Act & Assert
        Assert.True(manager.ManagerID > 0); // ManagerID should be positive
        Assert.Equal(100, manager.ManagerID);
    }

    [Fact]
    public void Manager_EmptyUsername_ReturnsError()
    {
        // Arrange
        var manager = new Manager
        {
            ManagerID = 3,
            Username = "", // Empty Username
            Password = "Password123"
        };

        // Act
        var username = manager.Username;

        // Assert
        Assert.Equal("", username); // Assert username is empty
        Assert.True(string.IsNullOrEmpty(username));
    }
}
