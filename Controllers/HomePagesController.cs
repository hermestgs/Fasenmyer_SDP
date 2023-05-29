using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Data;
using FasenmyerConference.Models;

namespace FasenmyerConference.Controllers
{
    public class HomePagesController : Controller
    {
        private readonly FasenmyerConferenceContext _context;

        public HomePagesController(FasenmyerConferenceContext context)
        {
            _context = context;
        }

        // GET: HomePages
        //public async Task<IActionResult> Index()
        //{
        //      return _context.HomePage != null ? 
        //                  View(await _context.HomePage.ToListAsync()) :
        //                  Problem("Entity set 'FasenmyerConferenceContext.HomePage'  is null.");
        //}

        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";

            var example = from e in _context.HomePage
                          select e;


            return View(example.ToList());
        }

        // GET: HomePages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // GET: HomePages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomePages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WelcomeArea,KeynoteIntro,SpotlightIntro, ContactInfo")] HomePage homePage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homePage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homePage);
        }

        // GET: HomePages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage.FindAsync(id);
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePage);
        }

        // POST: HomePages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WelcomeArea,KeynoteIntro,SpotlightIntro,ContactInfo")] HomePage homePage)
        {
            if (id != homePage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homePage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(homePage.Id))
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
            return View(homePage);
        }

        // GET: HomePages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // POST: HomePages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HomePage == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.HomePage'  is null.");
            }
            var homePage = await _context.HomePage.FindAsync(id);
            if (homePage != null)
            {
                _context.HomePage.Remove(homePage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomePageExists(int id)
        {
          return (_context.HomePage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
