/*
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Claims.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }
            return View(lectureClaim);
        }

        public IActionResult Create()
        {
            return View();
        }

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

                    // Optionally, store the file name in the database
                    // lectureClaim.DocumentPath = fileName; // Add this property to your model if needed
                }

                _context.Add(lectureClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lectureClaim);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }
            return View(lectureClaim);
        }

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

                        // Optionally, update the file path in the database
                        // lectureClaim.DocumentPath = fileName; // Add this property to your model if needed
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

        public async Task<IActionResult> Delete(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }
            return View(lectureClaim);
        }

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
    }
}

*/
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Claims.ToListAsync());
        }

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

      

        public IActionResult Create()
        {
            return View();
        }

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

                    // Optionally, store the file name in the database
                    // lectureClaim.DocumentPath = fileName; // Add this property to your model if needed
                }

                _context.Add(lectureClaim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ClaimsList));
            }
            return View(lectureClaim);
        }
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Retrieve the LectureClaim by its ClaimID
            var lectureClaim = _context.Claims.SingleOrDefault(e => e.ClaimID == id);

            if (lectureClaim == null)
            {
                return NotFound();
            }

            return View(lectureClaim);
        }




        /*  public async Task<IActionResult> Edit(int id)
          {
              var lectureClaim = await _context.Claims.FindAsync(id);
              if (lectureClaim == null)
              {
                  return NotFound();
              }
              return View(lectureClaim);
          } */

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

        public async Task<IActionResult> Delete(int id)
        {
            var lectureClaim = await _context.Claims.FindAsync(id);
            if (lectureClaim == null)
            {
                return NotFound();
            }
            return View(lectureClaim);
        }

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
        public IActionResult ClaimsList()
        {
            // Fetch claims from the database
            var claims = _context.Claims.ToList(); // Ensure this is the correct DbSet name

            // Pass claims to the view
            return View(claims);
        }

    }
}
