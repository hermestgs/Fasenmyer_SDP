using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Data;
using FasenmyerConference.Models;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace FasenmyerConference.Controllers
{
    [AllowAnonymous]
    public class PresentationsController : Controller
    {

        private readonly FasenmyerConferenceContext _context;

        public PresentationsController(FasenmyerConferenceContext context)
        {
            _context = context;
        }

        // GET: Presentations
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Presentations == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Presentation' is null.");
            }

            var presentations = from p in _context.Presentations
                         select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                string upd = searchString.ToLower();

                presentations = presentations.Where(s => s.PName.Contains(upd)
                || s.Id!.ToLower().Contains(upd)
                || s.PName.ToLower().Contains(upd)
                || s.Sponsor!.Contains(upd)
                || s.Sponsor.ToLower().Contains(upd)
                || s.Advisor!.Contains(upd)
                || s.Advisor.ToLower().Contains(upd)
                || s.Time!.Contains(upd)
                || s.Room!.Contains(upd)
                || s.Room.ToLower().Contains(upd.ToLower())
                || s.Student1!.Contains(upd)
                || s.Student1.ToLower().Contains(upd)
                || s.Student2!.Contains(upd)
                || s.Student2.ToLower().Contains(upd)
                || s.Student3!.Contains(upd)
                || s.Student3.ToLower().Contains(upd)
                || s.Student4!.Contains(upd)
                || s.Student4.ToLower().Contains(upd)
                || s.Major!.Contains(upd)
                || s.Major.ToLower().Contains(upd.ToLower()));
            }

            return View(await presentations.ToListAsync());
        }

        // GET: Presentations/Details/5
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

        // GET: Presentations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Presentations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PName,Time,Sponsor,Major,Room")] Presentations presentations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(presentations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(presentations);
        }

        // GET: Presentations/Edit/5
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

        // POST: Presentations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, Advisor,PName, Student1, Student2, Student3, Student4,Time,Sponsor,Major,Room")] Presentations presentations)
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

        // GET: Presentations/Delete/5
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

        // POST: Presentations/Delete/5
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

        private bool PresentationsExists(string id)
        {
          return _context.Presentations.Any(e => e.Id == id);
        }


        // GET: Presentations/Map
        public IActionResult Map()
        {
            return View();
        }



    }
}
