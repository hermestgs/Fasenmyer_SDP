using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Data;
using FasenmyerConference.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace FasenmyerConference.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly FasenmyerConferenceContext _context;
        Presentations imp = new Presentations();

        public AdminController(FasenmyerConferenceContext context)
        {
            _context = context;
        }


        // GET: Admin
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Presentations == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Presentation'  is null.");
            }

            var presentations = from p in _context.Presentations
                                select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                presentations = presentations.Where(s => s.PName!.Contains(searchString)
                || s.Id!.Contains(searchString)
                || s.PName.ToLower().Contains(searchString)
                || s.Sponsor!.Contains(searchString)
                || s.Sponsor.ToLower().Contains(searchString)
                || s.Advisor!.Contains(searchString)
                || s.Advisor.ToLower().Contains(searchString)
                || s.Time!.Contains(searchString)
                || s.Room!.Contains(searchString)
                || s.Room.ToLower().Contains(searchString)
                || s.Student1!.Contains(searchString)
                || s.Student1.ToLower().Contains(searchString)
                || s.Student2!.Contains(searchString)
                || s.Student2.ToLower().Contains(searchString)
                || s.Student3!.Contains(searchString)
                || s.Student3.ToLower().Contains(searchString)
                || s.Student4!.Contains(searchString)
                || s.Student4.ToLower().Contains(searchString)
                || s.Major!.ToLower().Contains(searchString)
                || s.Major.Contains(searchString)) ;
            }

            presentations = presentations.OrderBy(s => s.Id);

            return View(await presentations.ToListAsync());
        }

        // method for retrieving data from Excel Sheet
        //public async Task<List<Presentations>> Import(IFormFile file)
        public async Task<IActionResult> Import(IFormFile file)
        {
            //If import button is clicked with no file/null, just refresh.
            ViewData["res"] = "null";

            //Check if a file is being uploaded
            if (file != null && file.Length > 0)
            {
                //Support only excel sheets
                var supported = "xlsx";
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                //If xlsx extension, proceed to read and save data into database
                if (supported.Contains(fileExt))
                {
                    var list = new List<Presentations>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                list.Add(new Presentations
                                {
                                    Id = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    PName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Room = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    Time = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                    Sponsor = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                    Advisor = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                    Student1 = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                    Student2 = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    Student3 = worksheet.Cells[row, 9].Value.ToString().Trim(),
                                    Student4 = worksheet.Cells[row, 10].Value.ToString().Trim(),
                                    Major = worksheet.Cells[row, 11].Value.ToString().Trim()
                                });
                            }

                            for (int row = 2; row <= rowCount; row++)
                            {
                                imp.Id = worksheet.Cells[row, 1].Value.ToString().Trim();
                                imp.PName = worksheet.Cells[row, 2].Value.ToString().Trim();
                                imp.Room = worksheet.Cells[row, 3].Value.ToString().Trim();
                                imp.Time = worksheet.Cells[row, 4].Value.ToString().Trim();
                                imp.Sponsor = worksheet.Cells[row, 5].Value.ToString().Trim();
                                imp.Advisor = worksheet.Cells[row, 6].Value.ToString().Trim();
                                imp.Student1 = worksheet.Cells[row, 7].Value.ToString().Trim();
                                imp.Student2 = worksheet.Cells[row, 8].Value.ToString().Trim();
                                imp.Student3 = worksheet.Cells[row, 9].Value.ToString().Trim();
                                imp.Student4 = worksheet.Cells[row, 10].Value.ToString().Trim();
                                imp.Major = worksheet.Cells[row, 11].Value.ToString().Trim();


                                try
                                {
                                    _context.Add(imp);
                                    await _context.SaveChangesAsync();
                                }
                                catch (DBConcurrencyException)
                                {
                                    throw;
                                }
                            }

                            
                        }
                    }
                    ViewData["res"] = "success";
                }
                //If other than .xlsx, return error to View
                else
                {
                    ViewData["res"] = "extError";
                }
            }


            var presentations = from p in _context.Presentations
                                select p;
  
            return View(await presentations.ToListAsync());
            
        }


        // GET: Admin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Presentations == null)
            {
                return NotFound();
            }

            var presentations = await _context.Presentations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presentations == null)
            {
                return NotFound();
            }

            return View(presentations);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Advisor,PName,Time,Sponsor,Student1, Student2, Student3, Student4,Major,Room")] Presentations presentations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(presentations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(presentations);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Presentations == null)
            {
                return NotFound();
            }

            var presentations = await _context.Presentations.FindAsync(id);
            if (presentations == null)
            {
                return NotFound();
            }
            return View(presentations);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, PName, Advisor,Time,Sponsor,Student1,Student2, Student3,Student4 ,Major,Room")] Presentations presentations)
        {
            if (id != presentations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presentations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentationsExists(presentations.Id))
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
            return View(presentations);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Presentations == null)
            {
                return NotFound();
            }

            var presentations = await _context.Presentations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presentations == null)
            {
                return NotFound();
            }

            return View(presentations);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Presentations == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Presentations'  is null.");
            }
            var presentations = await _context.Presentations.FindAsync(id);
            if (presentations != null)
            {
                _context.Presentations.Remove(presentations);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteAll()
        {
            return View();
        }


		// POST: Admin/DeleteAll/5
		[HttpPost, ActionName("DeleteAllConfirmed")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteAllConfirmed()
		{

			IList<Presentations> list = new List<Presentations>();
			foreach (var item in _context.Presentations)
			{
				list.Add(item);

			}

			using (var context = _context)
			{
				context.Presentations.RemoveRange(list);
				context.SaveChanges();
			}


			return RedirectToAction(nameof(Index));
		}

		private bool PresentationsExists(string id)
        {
          return _context.Presentations.Any(e => e.Id == id);
        }
    }
}
