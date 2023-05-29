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
using System.Collections;


namespace FasenmyerConference.Controllers
{
    public class EventsController : Controller
    {
        private readonly FasenmyerConferenceContext _context;

        public EventsController(FasenmyerConferenceContext context)
        {
            _context = context;
        }

        [Route("Events")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/")]
        [HttpGet]
        //public IEnumerable<Presentations> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        public IEnumerable<EventPresentations> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var DBpresentations = _context.Presentations;
            foreach(var FEDCevent in DBpresentations) {
                if((DateTime.Parse(FEDCevent.Time!) >= start) && (DateTime.Parse(FEDCevent.Time!) <= end)){
                    ////Console.WriteLine(FEDCevent.Time);
                    EventPresentations test = new EventPresentations()
                    {
                        Room = FEDCevent.Room,
                        text = FEDCevent.PName!,
                        start = DateTime.Parse(FEDCevent.Time),
                        end = DateTime.Parse(FEDCevent.Time).AddMinutes(30),
                        Sponsor = FEDCevent.Sponsor!,
                        Advisor = FEDCevent.Advisor!,
                        Student1 = FEDCevent.Student1!,
                        Student2 = FEDCevent.Student2!,
                        Student3 = FEDCevent.Student3!,
                        Student4 = FEDCevent.Student4!,
                        resource = FEDCevent.Room,
                        Major = FEDCevent.Major!

                    };
                    yield return test;
                }
            }

            //return from e in _context.Presentations where !(DateTime.Parse(e.Time!) <= start) || (DateTime.Parse(e.Time!).AddMinutes(30) >= end) select e;
            //return from e in _context.Presentations where !((DateTime.Parse(e.Time!) <= start) || (DateTime.Parse(e.Time!) >= end)) select e;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/")]
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Presentations @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Presentations.Count() != 0)
            {
                @event.Id = _context.Presentations.OrderBy(x => x.Id).Last().Id + 1;
            }
            else
            {   
                // Changed since the ID is a string and not a int
                @event.Id = "0";
            }
            _context.Presentations.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }


        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Added the ToString() for the id
            var @event = await _context.Presentations.SingleOrDefaultAsync(m => m.Id == id.ToString());

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }


        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/update/{id}")]
        [HttpPut]
        //public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Presentations param)
        // Changed to internally generated serialized object
        public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] EventPresentations param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Added the ToString() for the id
            var @event = await _context.Presentations.SingleOrDefaultAsync(m => m.Id == id.ToString());
            if (@event == null)
            {
                return NotFound();
            }

            @event.PName = param.text; // Dependecy Inject maps to EventPresentation which maps to the @event from the model
            @event.Sponsor = param.Sponsor;
            @event.Student1 = param.Student1;
            @event.Student2 = param.Student2;
            @event.Student3 = param.Student3;
            @event.Student4 = param.Student4;
            @event.Major = param.Major;
            @event.Room = param.Room;
            @event.Advisor = param.Advisor;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/move/{id}")]
        [HttpPut]
        public async Task<IActionResult> MoveEvent([FromRoute] int id, [FromBody] EventMoveParams param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Added the ToString() for the id
            var @event = await _context.Presentations.SingleOrDefaultAsync(m => m.Id == id.ToString());
            if (@event == null)
            {
                return NotFound();
            }
            // Converting DateTime to string for Presentation object
            @event.Time = param.start.ToString();
            // Not needed
            //@event.end = param.end;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [Route("/api/background/delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Added the ToString() for the id
            var @event = await _context.Presentations.SingleOrDefaultAsync(m => m.Id == id.ToString());
            if (@event == null)
            {
                return NotFound();
            }

            _context.Presentations.Remove(@event);
            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        private bool EventExists(int id)
        {
            // Added the ToString() for the id
            return _context.Presentations.Any(e => e.Id == id.ToString());
        }

        public class EventMoveParams
        {
            public DateTime start { get; set; }
            public DateTime end { get; set; }
        }
    }

    public class EventPresentations
    {
        public int Id { get; set; }
        public string? text { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string? Sponsor { get; set; }
        public string? Student1 { get; set; }
        public string? Student2 { get; set; }
        public string? Student3 { get; set; }
        public string? Student4 { get; set; }
        public string? Major { get; set; }
        public string? Room { get; set; }
        public string? resource { get; set; }

        public string? Advisor { get; set; }
    }
}