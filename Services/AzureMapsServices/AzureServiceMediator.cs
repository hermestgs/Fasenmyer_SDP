namespace FasenmyerConference.Services.AzureMapsServices
{
    // Manages all the different Azure Service Objects, takes orders from main Azure Service object
    public class AzureServiceMediator
    {
        private string _opertionalDate;
        private bool _allPresentationsComplete = false;
        private AzureDBService _azureDBService;
        private AzureMapsController _azureMapsController;
        private PresentationObserverBuilder _ObserverBuilder;
        public AzureServiceMediator()
        {
            // initializing Azure Maps Creator API controller
            _azureMapsController = new AzureMapsController();

            // Setting up Postgresql database API controller
            _azureDBService = new AzureDBService();
        }

        public void loadAzureDBConfig(string inputConnectionString)
        {
            // Load config parameters into Azure DB service
            _azureDBService.loadConnectionString(inputConnectionString);
        }

        public void loadAzureAPIConfig(string AzureSubscriptionKey, string statesetId, string datasetId)
        {
            // Load config parameters into Azure DB service
            _azureMapsController.loadAzureAPIConfig(AzureSubscriptionKey, statesetId, datasetId);
        }


        public async Task<int> AzureServiceInitAsync()
        {

            // Establishing the observer and builder
            _ObserverBuilder = new PresentationObserverBuilder();

            // Grabbing presentation data from database
            // Will only grab the current date, unless the overloaded constructor is used
            _azureDBService.getPresentations(_ObserverBuilder);

            setOperationalDateTime();

            if (_ObserverBuilder.getSubscribers() != null)
            {
                // Checks how many elements in the observer listing
                _ObserverBuilder.setSubscribersCount();

                // Creating Builder, converting JSON to Presentation objects
                List<string> roomList = _ObserverBuilder.getSubscribersAllListing();

                // Assigning Feature-ID for each object in builder - needed to change the state of the rooms
                foreach (string room in roomList)
                {
                    string featureID = await _azureMapsController.GetFeatureIDAsync(room);
                    _ObserverBuilder.setFeatureID(room, featureID);
                    //Console.WriteLine("Feature-ID: {0}", featureID);
                }

                // Used for testing, by printing the Presentation objects
                //_ObserverBuilder.GetString();

                // Checks if all presentations are done
                _ObserverBuilder.PresentationsTiming();
            }

            setIsDone();

            return 0;
        }

        // Updates the state of the room objects in Azure Maps Creator by checking if the presentations are active
        public async Task PresentationUpdateAsync(bool updateState)
        {
            //List<string> activePresentations;
            Dictionary<string, string> activePresentations;

            // Grabbing a list of rooms within subscribers
            switch (updateState)
            {
                case true:
                    activePresentations = _ObserverBuilder.GetActivePresentations();
                    break;
                case false:
                    activePresentations = _ObserverBuilder.GetEndingPresentations();
                    break;
            };

            // Used for testing - CKC
            //foreach (var value in activePresentations)
            //{
            //    Console.WriteLine(value);
            //}

            // Iterate over dictionary and send update commands to Azure Maps API

            if ((activePresentations.Count() > 0) && (!_allPresentationsComplete))
            {
                //foreach (var room in activePresentations)
                foreach (KeyValuePair<string, string> presentation in activePresentations)
                    {
                    // presentation.Key = featureID (room); presentation.Value = Major (color)
                    await _azureMapsController.updatePresentationAsync(presentation.Key, presentation.Value, updateState);
                }
            }

            // Checks observer if presentations are done and sets it - controls main while loop sentinal
            setIsDone();
        }

        public int getSubscriberCount()
        {
            return _ObserverBuilder.getSubscriberCount();
        }

        // Checks observer if presentations are done and sets it
        public void setIsDone()
        {
            _allPresentationsComplete = _ObserverBuilder.arePresentationsDone();
        }

        public bool getIsDone()
        {
            return _allPresentationsComplete;
        }

        public void setOperationalDateTime()
        {
            _opertionalDate = DateTime.Now.Date.ToShortDateString();
        }

        public string getOperationalDate()
        {
            return _opertionalDate;
        }

    }
}
