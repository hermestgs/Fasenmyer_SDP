using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static UpdateMapState;

// Used for interacting with the Microsoft Azure Maps Creator API
namespace FasenmyerConference.Services.AzureMapsServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureMapsController : ControllerBase
    {
        private static HttpClient Client = new HttpClient();
        private string _AzureSubscriptionKey;
        private string _StateID;
        private string _DatasetId;

        public AzureMapsController()
        {

        }

        // Loads up variables used for communicating with Azure API
        public void loadAzureAPIConfig(string AzureSubscriptionKey, string statesetId, string datasetId)
        {
            _AzureSubscriptionKey = AzureSubscriptionKey;
            _StateID = statesetId;
            _DatasetId = datasetId;
        }

        // Updates the state of the room and changes the color depending on if there is a presentation or not
        [HttpPut]
        public async Task updatePresentationAsync(string unitNum, bool isON)
        {
            // URL link for Azure API interaction
            var URL = $"https://us.atlas.microsoft.com/featurestatesets/{_StateID}/featureStates/{unitNum}?api-version=2.0&subscription-key={_AzureSubscriptionKey}";

            // A JSON object used for sending state to Microsoft
            UpdateMapState JSONBody = new UpdateMapState()
            {
                states = new StatesMap[] {
                    new StatesMap() {
                        keyName = "presentation",
                        value = isON,
                        eventTimestamp = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddThh:mm:ss")
                    } }
            };

            // Turns the object into a JSON for sending to Azure cloud
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(JSONBody, options);
            
            // Encodes the header for sending to Azure cloud services
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Sends the request to Azure
            var result = await Client.PutAsync(URL, requestContent);

            // Print the results - will convert to object for status checking
            //Console.WriteLine(result);

            /*
            if (result.StatusCode.ToString() == "200")
            {
                _azureAPIlogger.LogInformation("Status-code: {0}, Azure Maps Creator map has been successfully updated...", result.StatusCode.ToString());
            }
            else
            {
                _azureAPIlogger.LogInformation("Status-code: {0}, Azure Maps Creator map has failed to update...", result.StatusCode.ToString());
            }
            */


        }

        // Updates the state of the room and changes the color depending on if there is a presentation or not
        // Overloaded for color coding based off major
        [HttpPut]
        public async Task updatePresentationAsync(string unitNum, string major, bool isON)
        {
            //Console.WriteLine("FeatureID: {0}, and Major: {1}, and Bool: {2}", unitNum, major, isON);
            // URL link for Azure API interaction
            var URL = $"https://us.atlas.microsoft.com/featurestatesets/{_StateID}/featureStates/{unitNum}?api-version=2.0&subscription-key={_AzureSubscriptionKey}";

            // A JSON object used for sending state to Microsoft
            UpdateMapState JSONBody = new UpdateMapState()
            {
                states = new StatesMap[] {
                    new StatesMap() {
                        keyName = major,
                        value = isON,
                        eventTimestamp = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddThh:mm:ss")
                    } }
            };

            // Turns the object into a JSON for sending to Azure cloud
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(JSONBody, options);

            // Encodes the header for sending to Azure cloud services
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Sends the request to Azure
            var result = await Client.PutAsync(URL, requestContent);

            // Print the results - will convert to object for status checking
            //Console.WriteLine(result);


        }

        // Gets the feature-ID, a unique number issued for each room. Needed for changing the state of the room's color
        [HttpGet]
        public async Task<string> GetFeatureIDAsync(string unitNum)
        {
            string returnStr = string.Empty;

            // If the room is null, don't do anything
            if (string.IsNullOrEmpty(unitNum))
            {
                return (returnStr);
            }

            // The URL needed for the 'get' request
            var URL = $"https://us.atlas.microsoft.com/wfs/datasets/{_DatasetId}/collections/unit/items?subscription-key={_AzureSubscriptionKey}&api-version=2.0&name={unitNum}";

            // Sends a get request and receives the data
            var result = await Client.GetAsync(URL);

            // Transform inbound http string to a normal string
            var responseBody = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Takes the JSON object and converts it to a C# class object
            var getUnitData = JsonSerializer.Deserialize<JSONGetUnit>(responseBody);

            // Goes through new object and grabs the feature-ID from request
            foreach (var item in getUnitData.features)
            {
                if (item.properties.name == unitNum)
                {
                    returnStr = item.id;
                }
            }

            //Console.WriteLine($"Feature-ID: {returnStr}");
            
            // Returns the feature-ID, so it can be set into a Presentation object
            return (returnStr);
        }
    }
}

// The 'get' request packs this object with the request results
public class JSONGetUnit
{

    public string type { get; set; }
    public string ontology { get; set; }
    public Feature[] features { get; set; }
    public int numberReturned { get; set; }
    public Link[] links { get; set; }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
        public string id { get; set; }
        public string featureType { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public float[][][] coordinates { get; set; }
    }

    public class Properties
    {
        public string originalId { get; set; }
        public string categoryId { get; set; }
        public bool isOpenArea { get; set; }
        public bool isRoutable { get; set; }
        public string levelId { get; set; }
        public object[] occupants { get; set; }
        public string addressId { get; set; }
        public string name { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
    }

}

// Class used for updating the state of a room
public class UpdateMapState
{

    public StatesMap[]? states { get; set; }

    public class StatesMap
    {
        public string? keyName { get; set; }
        public bool value { get; set; }
        public string? eventTimestamp { get; set; }
    }

}



