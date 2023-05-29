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

namespace FasenmyerConference.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly FasenmyerConferenceContext _context;

        public SchedulesController(FasenmyerConferenceContext context)
        {
            _context = context;
        }

        //return View(await _context.Schedule.ToListAsync());
        // GET: Schedules
        // this method now returns the schedule in order
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";
         
            var schedule = from s in _context.Schedule
                           select s;
            switch (sortOrder)
            {
                case "time_desc":
                    schedule = schedule.OrderByDescending(s => s.Order);
                    break;
                default:
                    schedule = schedule.OrderBy(s => s.Order);
                    break;
            }
           
            return View( schedule.ToList());
        }

        [AllowAnonymous]
        public ActionResult ScheduleOfEvents(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";

            var schedule = from s in _context.Schedule
                           select s;
            switch (sortOrder)
            {
                case "time_desc":
                    schedule = schedule.OrderByDescending(s => s.Order);
                    break;
                default:
                    schedule = schedule.OrderBy(s => s.Order);
                    break;
            }

            return View(schedule.ToList());
        }


        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Order,Time,Event,Location")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Order,Time,Event,Location")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Schedule == null)
            {
                return Problem("Entity set 'FasenmyerConferenceContext.Schedule'  is null.");
            }
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(string id)
        {
          return _context.Schedule.Any(e => e.Id == id);
        }
    }
}
