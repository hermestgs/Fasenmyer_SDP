using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Data;
using FasenmyerConference.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace FasenmyerConference.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Spotlight_SponsorController : Controller
    {
        private readonly FasenmyerConferenceContext _context;
        IWebHostEnvironment _environment;

        public Spotlight_SponsorController(FasenmyerConferenceContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Spotlight_Sponsor
        public async Task<IActionResult> Index()
        {
              return View(await _context.Spotlight_Sponsor.ToListAsync());
        }

        [AllowAnonymous]
        public ActionResult SponsorView()
        {
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";

            var example = from e in _context.Spotlight_Sponsor
                          select e;


            return View(example.ToList());
        }
        //public async Task<IActionResult> SponsorView()
        //{
        //    return View(await _context.Spotlight_Sponsor.ToListAsync());
        //}


        // GET: Spotlight_Sponsor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Spotlight_Sponsor == null)            {
                return NotFound();
            }

            var spotlight_Sponsor = await _context.Spotlight_Sponsor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spotlight_Sponsor == null)
            {
                return NotFound();
            }

            return View(spotlight_Sponsor);
        }

        // GET: Spotlight_Sponsor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spotlight_Sponsor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Sponsor_Name,Details")] Spotlight_Sponsor spotlight_Sponsor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spotlight_Sponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spotlight_Sponsor);
        }

        // GET: Spotlight_Sponsor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Spotlight_Sponsor == null)
            {
                return NotFound();
            }

            var spotlight_Sponsor = await _context.Spotlight_Sponsor.FindAsync(id);
            if (spotlight_Sponsor == null)
            {
                return NotFound();
            }
            return View(spotlight_Sponsor);
        }

        // POST: Spotlight_Sponsor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, Sponsor_Name,Details")] Spotlight_Sponsor spotlight_Sponsor)
        {
            if (id != spotlight_Sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spotlight_Sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Spotlight_SponsorExists(spotlight_Sponsor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spotlight_Sponsor);
        }

        // GET: Spotlight_Sponsor/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Spotlight_Sponsor == null)
            {
                return NotFound();
            }

            var spotlight_Sponsor = await _context.Spotlight_Sponsor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spotlight_Sponsor == null)
            {
                return NotFound();
            }

            return View(spotlight_Sponsor);
        }

        // POST: Spotlight_Sponsor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Spotlight_Sponsor == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Spotlight_Sponsor'  is null.");
            }
            var spotlight_Sponsor = await _context.Spotlight_Sponsor.FindAsync(id);
            if (spotlight_Sponsor != null)
            {
                _context.Spotlight_Sponsor.Remove(spotlight_Sponsor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Spotlight_SponsorExists(string id)
        {
          return _context.Spotlight_Sponsor.Any(e => e.Id == id);
        }


        // method for retrieving photo upload
        //public async Task<List<Presentations>> Upload(IFormFile file)
        public async Task<IActionResult> Upload(IFormFile file)
        {

            //If null, upload button is clicked
            ViewData["res"] = "null";

            //If a file is uploaded
            if (file != null && file.Length > 0)
            {
                var imagePath = @"\Upload\Images\";
                var uploadPath = _environment.WebRootPath + imagePath;

                //Support only jpg extensions
                var supported = "jpg";
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                //If jpg extension, proceed to save photo into server
                if (supported.Contains(fileExt))
                {
                    //Create Directory
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    //Create Uniq file name
                    //var uniqFileName = Guid.NewGuid().ToString();
                    var uniqFileName = "SPic";
                    var filename = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".")[1].ToLower());
                    string fullPath = uploadPath + filename;
                    imagePath = imagePath + @"\";
                    var filePath = @".." + Path.Combine(imagePath, filename);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    ViewData["FileLocation"] = filePath;
                    ViewData["res"] = "success";
                }
                //If other than .jpg, return error to View
                else
                {
                    ViewData["res"] = "extError";
                }

            }

            return View(await _context.Spotlight_Sponsor.ToListAsync());

        }


    }
}
