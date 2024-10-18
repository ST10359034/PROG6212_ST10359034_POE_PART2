// Emil Fabel
// ST10359034
// Group 1

// References:
// https://www.youtube.com/watch?v=jT8eA9A7qXE&list=PL_T9TZ4DVz4vAl8jl_18Y_XCX76hgzHiS&index=5
// https://www.youtube.com/watch?v=bqyZiwXOMH0&list=PL_T9TZ4DVz4vAl8jl_18Y_XCX76hgzHiS&index=7
// https://www.youtube.com/watch?v=DTQMTB2ghDM&list=PL_T9TZ4DVz4vAl8jl_18Y_XCX76hgzHiS&index=8
// https://www.youtube.com/watch?v=d2gXwDqNVrY
// https://github.com/rst9454/CrudApplication
// https://learn.microsoft.com/en-us/ef/core/
// https://www.w3schools.com/cs/index.php
// https://www.youtube.com/watch?v=aumcaBkprsA

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS.Data;
using CMCS.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    public class LectureClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LectureClaimsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Display list of all claims
        public async Task<IActionResult> Index()
        {
            return View(await _context.Claims.ToListAsync());
        }

        //---------------------------------------------------------------------

        // Update the status of a claim
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }

            lectureClaim.Status = status; // Update the status
            _context.Update(lectureClaim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //---------------------------------------------------------------------

        // Show claim creation view
        public IActionResult Create()
        {
            return View();
        }

        //---------------------------------------------------------------------

        // Create new claim with optional document upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LectureClaim lectureClaim)
        {
            if (ModelState.IsValid)
            {
                if (lectureClaim.ClaimDocument != null)
                {
                    string folder = "SupportingDocument/Document";
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(lectureClaim.ClaimDocument.FileName);
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        await lectureClaim.ClaimDocument.CopyToAsync(fileStream);
                    }

                   
                }

                _context.Add(lectureClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ClaimsList));
            }
            return View(lectureClaim);
        }

        //---------------------------------------------------------------------

        // Show claim details by ID for Moderators/Managers
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lectureClaim = _context.Claims.SingleOrDefault(e => e.ClaimID == id);

            if (lectureClaim == null)
            {
                return NotFound();
            }

            return View(lectureClaim);
        }

        //---------------------------------------------------------------------

        // Show claim details by ID for Lecturers
        public IActionResult Details1(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lectureClaim = _context.Claims.SingleOrDefault(e => e.ClaimID == id);

            if (lectureClaim == null)
            {
                return NotFound();
            }

            return View(lectureClaim);
        }

        //---------------------------------------------------------------------

        // Edit claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LectureClaim lectureClaim)
        {
            if (id != lectureClaim.ClaimID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (lectureClaim.ClaimDocument != null)
                    {
                        string folder = "SupportingDocument/Document";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(lectureClaim.ClaimDocument.FileName);
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                        using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                        {
                            await lectureClaim.ClaimDocument.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(lectureClaim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Claims.Any(e => e.ClaimID == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lectureClaim);
        }

        //---------------------------------------------------------------------

        // Confirm claim deletion
        public async Task<IActionResult> Delete(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }
            return View(lectureClaim);
        }

        //---------------------------------------------------------------------

        // Delete claim from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim != null)
            {
                _context.Claims.Remove(lectureClaim);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //---------------------------------------------------------------------

        // Display claims list
        public IActionResult ClaimsList()
        {
            var claims = _context.Claims.ToList(); // Fetch claims from the database
            return View(claims);
        }
    }
}

