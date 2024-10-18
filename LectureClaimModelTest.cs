
using CMCS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CMCS.test { }
public class LectureClaimTests
{
    [Fact]
    public void LectureClaim_ValidModel_ReturnsTrue()
    {
        // Arrange
        var claim = new LectureClaim
        {
            ClaimID = 1,
            LecturerName = "John",
            LecturerSurname = "Doe",
            LecturerEmail = "john.doe@example.com",
            ClaimDate = DateTime.Now,
            HoursWorked = 10,
            HourlyRate = 20.00m,
            Status = "Pending"
        };

        // Act
        var validationResults = ValidateModel(claim);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void LectureClaim_MissingRequiredFields_ReturnsErrors()
    {
        // Arrange
        var claim = new LectureClaim
        {
            ClaimID = 1,
            // Missing LecturerName, LecturerSurname, and LecturerEmail
            ClaimDate = DateTime.Now,
            HoursWorked = 10,
            HourlyRate = 20.00m,
            Status = "Pending"
        };

        // Act
        var validationResults = ValidateModel(claim);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Equal(3, validationResults.Count); // Expecting 3 validation errors
    }

    [Fact]
    public void LectureClaim_InvalidEmail_ReturnsError()
    {
        // Arrange
        var claim = new LectureClaim
        {
            ClaimID = 1,
            LecturerName = "John",
            LecturerSurname = "Doe",
            LecturerEmail = "invalid-email", // Invalid email format
            ClaimDate = DateTime.Now,
            HoursWorked = 10,
            HourlyRate = 20.00m,
            Status = "Pending"
        };

        // Act
        var validationResults = ValidateModel(claim);

        // Assert
        Assert.Single(validationResults); // Expecting 1 validation error
        Assert.Equal("The Email field is not a valid e-mail address.", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void LectureClaim_InvalidHoursWorked_ReturnsError()
    {
        // Arrange
        var claim = new LectureClaim
        {
            ClaimID = 1,
            LecturerName = "John",
            LecturerSurname = "Doe",
            LecturerEmail = "john.doe@example.com",
            ClaimDate = DateTime.Now,
            HoursWorked = -5, // Invalid hours worked
            HourlyRate = 20.00m,
            Status = "Pending"
        };

        // Act
        var validationResults = ValidateModel(claim);

        // Assert
        Assert.Single(validationResults); // Expecting 1 validation error
        Assert.Equal("Please enter valid hours worked", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void LectureClaim_InvalidHourlyRate_ReturnsError()
    {
        // Arrange
        var claim = new LectureClaim
        {
            ClaimID = 1,
            LecturerName = "John",
            LecturerSurname = "Doe",
            LecturerEmail = "john.doe@example.com",
            ClaimDate = DateTime.Now,
            HoursWorked = 10,
            HourlyRate = -15, // Invalid hourly rate
            Status = "Pending"
        };

        // Act
        var validationResults = ValidateModel(claim);

        // Assert
        Assert.Single(validationResults); // Expecting 1 validation error
        Assert.Equal("Please enter valid hourly rate", validationResults[0].ErrorMessage);
    }

    private List<ValidationResult> ValidateModel(LectureClaim claim)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(claim);
        Validator.TryValidateObject(claim, validationContext, validationResults, true);
        return validationResults;
    }
}
