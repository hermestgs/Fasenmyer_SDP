namespace FasenmyerConference.Services.AzureMapsServices
{
    // This class is used for loading values from the config file (appsettings.Development.json)
    public class AzureConfig
    {
        public string? FasenmyerConferenceContext { get; set; }
        public string? AzureSubscriptionKey { get; set; }
        public string? tilesetId { get; set; }
        public string? statesetId { get; set; }
        public string? datasetId { get; set; }
    }
}
