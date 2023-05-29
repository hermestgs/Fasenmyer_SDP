using FasenmyerConference.Data;
using FasenmyerConference.Models;
using FasenmyerConference.Services.AzureMapsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FasenmyerConference.Controllers
{
    public class BurkeMapController : Controller
    {
        // Dependency inject for logger and database operations
        private readonly FasenmyerConferenceContext _WebAppDB;
        private readonly IOptions<AzureConfig> _AzureMapsConfig;

        public BurkeMapController(IOptions<AzureConfig> inputConfig, FasenmyerConferenceContext inputDB)
        {
            // Loads the values from the config file
            _WebAppDB = inputDB;
            _AzureMapsConfig = inputConfig;   

            // Setting the variables used in the Azure Maps Creator view
            AzureSubscriptionKey = _AzureMapsConfig.Value.AzureSubscriptionKey;
            tilesetId = _AzureMapsConfig.Value.tilesetId;
            statesetId = _AzureMapsConfig.Value.statesetId;

        }

        [AllowAnonymous]
        public IActionResult MapIndex()
        {
            // Loads the presentation info for the map pop-up
            loadDB();
            return View();
        }

        // Grabs presentation data from the database
        private void loadDB()
        {
            var presentations = new List<DBData>();

            // Grabs the presentation info from database
            var DBpresentations = _WebAppDB.Presentations;

            foreach (var aPresentation in DBpresentations)
            {
                DateTime dateAndTime = DateTime.Parse(aPresentation.Time.ToString()!);
                TimeSpan presentationTime = TimeSpan.Parse(dateAndTime.ToString("HH:mm:ss"));

                if (dateAndTime.ToShortDateString() == DateTime.Now.ToShortDateString())
                {

                    if ((DateTime.Now.TimeOfDay >= presentationTime)
                        && (DateTime.Now.TimeOfDay < presentationTime.Add(TimeSpan.FromMinutes(30))))
                    {
                        presentations.Add(
                            new DBData()
                            {
                                Room = aPresentation.Room.PadLeft(3, '0'),
                                ProjectName = aPresentation.PName!,
                                Sponsor = aPresentation.Sponsor!,
                                Advisor = aPresentation.Advisor!,
                                Student1 = aPresentation.Student1!,
                                Student2 = aPresentation.Student2!,
                                Student3 = aPresentation.Student3!,
                                Student4 = aPresentation.Student4!,
                                Major = aPresentation.Major!,
                            });

                    }
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            serializedJSON = JsonSerializer.Serialize(presentations, options);
            
            // Used for testing - CKC
            //Console.WriteLine(serializedJSON);
        }

        // Class methods used for transmitting data to Azure Maps Creator views

        [ViewData]
        public string? AzureSubscriptionKey { get; set; }
        [ViewData]
        public string? tilesetId { get; set; }
        [ViewData]
        public string? statesetId { get; set; }
        [ViewData]
        public string? serializedJSON { get; set; }

        public class DBData
        {
            public string? Room { get; set; }
            public string? ProjectName { get; set; }
            public string? Sponsor { get; set; }
            public string? Advisor { get; set; }
            public string? Student1 { get; set; }
            public string? Student2 { get; set; }
            public string? Student3 { get; set; }
            public string? Student4 { get; set; }
            public string? Major { get; set; }
        }


    }
}
