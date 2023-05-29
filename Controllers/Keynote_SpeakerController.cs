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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FasenmyerConference.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Keynote_SpeakerController : Controller
    {
        private readonly FasenmyerConferenceContext _context;
        IWebHostEnvironment _environment;

        public Keynote_SpeakerController(FasenmyerConferenceContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        [AllowAnonymous]
        public ActionResult KeynoteBio()
        {
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";

            var example = from e in _context.Keynote_Speaker
                          select e;


            return View(example.ToList());
        }

        //// GET: Keynote_Speaker
        public async Task<IActionResult> Index()
        {
            return View(await _context.Keynote_Speaker.ToListAsync());
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> KeynoteBio()
        //{
        //    return View(await _context.Keynote_Speaker.ToListAsync());
        //}

        [AllowAnonymous]
        // GET: Keynote_Speaker/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Keynote_Speaker == null)
            {
                return NotFound();
            }
            var keynote_Speaker = await _context.Keynote_Speaker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keynote_Speaker == null)
            {
                return NotFound();
            }
            return View(keynote_Speaker);
        }
        // GET: Keynote_Speaker/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Keynote_Speaker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KName,Bio")] Keynote_Speaker keynote_Speaker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keynote_Speaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keynote_Speaker);
        }
        // GET: Keynote_Speaker/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Keynote_Speaker == null)
            {
                return NotFound();
            }
            var keynote_Speaker = await _context.Keynote_Speaker.FindAsync(id);
            if (keynote_Speaker == null)
            {
                return NotFound();
            }
            return View(keynote_Speaker);
        }
        // POST: Keynote_Speaker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,KName,Bio")] Keynote_Speaker keynote_Speaker)
        {
            if (id != keynote_Speaker.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keynote_Speaker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Keynote_SpeakerExists(keynote_Speaker.Id))
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
            return View(keynote_Speaker);
        }
        // GET: Keynote_Speaker/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Keynote_Speaker == null)
            {
                return NotFound();
            }
            var keynote_Speaker = await _context.Keynote_Speaker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keynote_Speaker == null)
            {
                return NotFound();
            }
            return View(keynote_Speaker);
        }
        // POST: Keynote_Speaker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Keynote_Speaker == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Keynote_Speaker'  is null.");
            }
            var keynote_Speaker = await _context.Keynote_Speaker.FindAsync(id);
            if (keynote_Speaker != null)
            {
                _context.Keynote_Speaker.Remove(keynote_Speaker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool Keynote_SpeakerExists(string id)
        {
            return _context.Keynote_Speaker.Any(e => e.Id == id);
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
                    var uniqFileName = "KPic";
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

            return View(await _context.Keynote_Speaker.ToListAsync());

        }


    }
}