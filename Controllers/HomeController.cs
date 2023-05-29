using FasenmyerConference.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using FasenmyerConference.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Data;
using FasenmyerConference.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FasenmyerConference.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FasenmyerConferenceContext _context;

        public HomeController(ILogger<HomeController> logger, FasenmyerConferenceContext context)
        {
            _context = context;
            _logger = logger;

            foreach (var item in context.Conference) { 

                ConferenceDate = item.Date;

            }
        }

        // original Index IactionResult
        // public IActionResult Index()
        //{
        //   return View();
        //}


        //return View(await _context.Schedule.ToListAsync());
        // GET: Schedules
        // this method now returns the schedule in order
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";

            var hPage = from h in _context.HomePage
                        select h;

            return View(hPage.ToList());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ViewData]
        public string? ConferenceDate { get; set; }
    }

}

