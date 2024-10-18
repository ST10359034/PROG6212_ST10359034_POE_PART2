using CMCS.Controllers;
using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CMCS.Tests
{
    public class LectureClaimsControllerTests
    {
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly LectureClaimsController _controller;

        public LectureClaimsControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock IWebHostEnvironment
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Directory.GetCurrentDirectory());

            _controller = new LectureClaimsController(_context, _mockWebHostEnvironment.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithClaims()
        {
            // Arrange
            _context.Claims.Add(new LectureClaim { LecturerName = "John", LecturerSurname = "Doe" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            var model = Assert.IsAssignableFrom<List<LectureClaim>>(result.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Create_ValidModel_AddsClaim()
        {
            // Arrange
            var claim = new LectureClaim
            {
                LecturerName = "John",
                LecturerSurname = "Doe",
                LecturerEmail = "john.doe@example.com",
                ClaimDate = DateTime.Now,
                HoursWorked = 5,
                HourlyRate = 15.0m,
                Status = "Pending",
                ClaimDocument = null 
            };

            // Act
            var result = await _controller.Create(claim) as RedirectToActionResult;

            // Assert
            Assert.Equal("ClaimsList", result.ActionName);
            Assert.Single(await _context.Claims.ToListAsync());
        }

        [Fact]
        public async Task UpdateStatus_ValidId_UpdatesStatus()
        {
            // Arrange
            var claim = new LectureClaim
            {
                LecturerName = "John",
                LecturerSurname = "Doe",
                LecturerEmail = "john.doe@example.com",
                ClaimDate = DateTime.Now,
                HoursWorked = 5,
                HourlyRate = 15.0m,
                Status = "Pending"
            };
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.UpdateStatus(claim.ClaimID, "Approved") as RedirectToActionResult;

            // Assert
            Assert.Equal("Index", result.ActionName);
            var updatedClaim = await _context.Claims.FindAsync(claim.ClaimID);
            Assert.Equal("Approved", updatedClaim.Status);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_RemovesClaim()
        {
            // Arrange
            var claim = new LectureClaim
            {
                LecturerName = "John",
                LecturerSurname = "Doe",
                LecturerEmail = "john.doe@example.com",
                ClaimDate = DateTime.Now,
                HoursWorked = 5,
                HourlyRate = 15.0m,
                Status = "Pending"
            };
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(claim.ClaimID) as RedirectToActionResult;

            // Assert
            Assert.Equal("Index", result.ActionName);
            Assert.Null(await _context.Claims.FindAsync(claim.ClaimID));
        }

       
      

        [Fact]
        public async Task Delete_ValidId_ReturnsViewWithClaim()
        {
            // Arrange
            var claim = new LectureClaim
            {
                LecturerName = "John",
                LecturerSurname = "Doe",
                LecturerEmail = "john.doe@example.com",
                ClaimDate = DateTime.Now,
                HoursWorked = 5,
                HourlyRate = 15.0m,
                Status = "Pending"
            };
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(claim.ClaimID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var model = Assert.IsAssignableFrom<LectureClaim>(viewResult.ViewData.Model); 
            Assert.Equal(claim.ClaimID, model.ClaimID);
        }
    }
}

