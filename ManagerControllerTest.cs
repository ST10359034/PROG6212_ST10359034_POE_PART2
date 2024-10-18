using CMCS.Controllers;
using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class ManagerControllerTests
{
    [Fact]
    public void ManagerLogin_ReturnsView()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var controller = new ManagerController(mockContext.Object);

        // Act
        var result = controller.ManagerLogin();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName); // Default view name returned
    }

    [Fact]
    public void ManagerLogin_ValidCredentials_RedirectsToLectureClaimsIndex()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var controller = new ManagerController(mockContext.Object);
        var manager = new Manager { Username = "Moderator", Password = "12345" }; // Valid credentials

        // Act
        var result = controller.ManagerLogin(manager);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("LectureClaims", redirectResult.ControllerName);
    }

    [Fact]
    public void ManagerLogin_InvalidCredentials_ReturnsViewWithError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var controller = new ManagerController(mockContext.Object);
        var manager = new Manager { Username = "InvalidUser", Password = "WrongPassword" }; // Invalid credentials

        // Act
        var result = controller.ManagerLogin(manager);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
        Assert.Equal(1, controller.ModelState.ErrorCount);
        Assert.Equal("Invalid login attempt.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task CreateManager_ValidManager_AddsToDatabase()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<Manager>>();
        mockContext.Setup(m => m.Managers).Returns(mockDbSet.Object);

        var controller = new ManagerController(mockContext.Object);
        var manager = new Manager { Username = "NewManager", Password = "SecurePassword" };

        // Act
        var result = await controller.CreateManager(manager);

        // Assert
        mockDbSet.Verify(m => m.Add(It.IsAny<Manager>()), Times.Once());
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public void Logout_RedirectsToManagerLogin()
    {
        // Arrange
        var mockContext = new Mock<ApplicationDbContext>();
        var controller = new ManagerController(mockContext.Object);

        // Act
        var result = controller.Logout();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ManagerLogin", redirectResult.ActionName);
    }
}
