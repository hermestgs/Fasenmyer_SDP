namespace FasenmyerConference.Services.AzureMapsServices
{
    // Observes Presentation objects and holds a builder sub-class
    public class PresentationObserverBuilder
    {
        private int TimeBuffer = 5; // 5 minutes before and after presentation start and projected end time
        private int _presentationCount = 0;
        private int _totalPresentations = -1;
        private PresentationBuilder _builder;
        private List<Presentation>? _Subscribers = null;
        private Dictionary<string, string> _Presentations;
        //private List<string> _Presentations;

        // Contructors and Builders
        public PresentationObserverBuilder()
        {
            RefreshBuilderAndSubscribers();
        }

        public void RefreshBuilderAndSubscribers()
        {
            int _presentationCount = 0;
            int _totalPresentations = -1;
            _Subscribers = new List<Presentation>();
            _builder = new PresentationBuilder();
        }

        public void DestoryBuilder()
        {
            _builder = null;
        }

        // Getters
        public PresentationBuilder getBuilder()
        {
            return _builder;
        }

        public List<Presentation> getSubscribers()
        {
            return _Subscribers;
        }

        public bool arePresentationsDone()
        {
            return (_presentationCount == _totalPresentations);
        }

        // Setters
        public void setSubscribers(List<Presentation> input)
        {
            _Subscribers = input;
        }

        public void addToList()
        {
            _Subscribers.Add(_builder.GetPresentation());
        }

        public void setSubscribersCount()
        {
            _totalPresentations = _Subscribers.Count;
        }

        public int getSubscriberCount()
        {
            return _totalPresentations;
        }

        // Other Methods

        public void ErrorOutputBuilder()
        {
            //Console.WriteLine("No presentations...");
        }

        // Checks if the presentation is about to start and changes color if it is
        //public List<string> GetActivePresentations()
        public Dictionary<string, string> GetActivePresentations()
        {

            //_Presentations = new List<string>();
            // <FeatureID (Room), Major>
            _Presentations = new Dictionary<string, string>();

            if (this.getSubscriberCount() > 0)
            {
                foreach (var APresentation in _Subscribers)
                {
                    if (APresentation.isPresenting != true)
                    {

                        TimeSpan presentationStartTime = TimeSpan.Parse(APresentation.StartTime);

                        if (DateTime.Now.TimeOfDay >= presentationStartTime.Subtract(TimeSpan.FromMinutes(TimeBuffer))
                                && (DateTime.Now.TimeOfDay <= presentationStartTime.Add(TimeSpan.FromMinutes(TimeBuffer))))
                        {
                            //_Presentations.Add(APresentation.FeatureID);
                            _Presentations.Add(APresentation.FeatureID, APresentation.Major);
                            APresentation.isPresenting = true;
                        }
                    }
                }
            }

            return _Presentations;

        }

        // Checks if the presentation is about to end and changes color back if it is
        //public List<string> GetEndingPresentations()
        public Dictionary<string, string> GetEndingPresentations()
        {
            //_Presentations = new List<string>();
            // <FeatureID (Room), Major>
            _Presentations = new Dictionary<string, string>();

            if (this.getSubscriberCount() > 0) 
            {
                foreach (var APresentation in _Subscribers)
                {
                    if (APresentation.completedPresentation != true)
                    {
                        TimeSpan presentationEndTime = TimeSpan.Parse(APresentation.EndTime);
                        if (DateTime.Now.TimeOfDay >= presentationEndTime.Subtract(TimeSpan.FromMinutes(TimeBuffer))
                            && (DateTime.Now.TimeOfDay <= presentationEndTime.Add(TimeSpan.FromMinutes(TimeBuffer))))
                        {

                            //_Presentations.Add(APresentation.FeatureID);
                            _Presentations.Add(APresentation.FeatureID, APresentation.Major);
                            APresentation.isPresenting = false;
                            APresentation.completedPresentation = true;
                            _presentationCount++;
                        }
                    }
                }
            }

            return _Presentations;

        }

        // Checks if presentations have alrady been completed
        public void PresentationsTiming()
        {

            foreach (var APresentation in _Subscribers)
            {
                if (APresentation.completedPresentation != true)
                {
                    TimeSpan presentationEndTime = TimeSpan.Parse(APresentation.EndTime);
                    if (DateTime.Now.TimeOfDay > presentationEndTime.Add(TimeSpan.FromMinutes(10)))
                    {
                        APresentation.isPresenting = false;
                        APresentation.completedPresentation = true;
                        _presentationCount++;
                    }
                }
            }

        }

        // Returns a list of Presentation objects
        public List<string> getSubscribersAllListing()
        {
            List<string> roomListing = new List<string>();

            if (_Subscribers != null)
            {
                foreach (var apresentation in _Subscribers)
                {
                    roomListing.Add(apresentation.room);
                }
            }
            else
            {
                ErrorOutputBuilder();
            }
            return roomListing;
        }

        // Converts the subscribers (Presentations) and prints them as string
        public void GetString()
        {

            if (_Subscribers != null)
            {
                foreach (var apresentation in _Subscribers)
                {
                    // Used for testing
                    /*
                    Console.WriteLine("\nRoom-Number: {0} " +
                        "DateTime: {1}, Date: {2}, Start-Time: {3}, End-Time: {4}" +
                        "Feature-ID: {5}, IsPresenting: {6}, Completed-Presentation: {7}\n",
                        apresentation.room,
                        apresentation.rawDateTime,
                        apresentation.Date,
                        apresentation.Major,
                        apresentation.StartTime,
                        apresentation.EndTime,
                        apresentation.FeatureID,
                        apresentation.isPresenting,
                        apresentation.completedPresentation
                        );
                    */
                }

            }
            else
            {
                ErrorOutputBuilder();
            }
        }

        // Sets the feature-ID for each Presentation object. Needed to update the room's state on Azure Maps cloud
        public void setFeatureID(string room, string inputFeatureID)
        {
            if (_Subscribers != null)
            {
                foreach (var apresentation in _Subscribers)
                {
                    if (apresentation.room == room)
                    {
                        apresentation.FeatureID = inputFeatureID;
                    }
                }
            }
            else
            {
                ErrorOutputBuilder();
            }
        }
    }

    // The builder sub-class, it constructs Presentations
    public class PresentationBuilder
    {
        private Presentation? _currentBuild;
        public PresentationBuilder()
        {
            _currentBuild = new Presentation();
        }

        // Getters
        public Presentation GetPresentation()
        {
            return _currentBuild;
        }

        // Setters
        private void setDate()
        {
            _currentBuild.Date = _currentBuild.rawDateTime.ToShortDateString();
        }

        private void setStartEndTimes()
        {
            _currentBuild.StartTime = _currentBuild.rawDateTime.ToString("HH:mm:ss");
            _currentBuild.EndTime = _currentBuild.rawDateTime.AddMinutes(30).ToString("HH:mm:ss");
        }

        private void setPresentationStatus()
        {
            _currentBuild.isPresenting = false;
        }

        private void setCompletedPresentationStatus()
        {
            _currentBuild.completedPresentation = false;
        }

        // Other Methods
        public void UpdateDateTime()
        {
            setDate();
            setStartEndTimes();
            setPresentationStatus();
            setCompletedPresentationStatus();
        }

        public void NewPresentation()
        {
            _currentBuild = new Presentation();
        }

        public void DestroyPresentation()
        {
            _currentBuild = null;
        }

    }
}

public class Presentation
{
    public string? room { get; set; }
    public DateTime rawDateTime { get; set; }
    public string? Date { get; set; }
    public string? Major { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? FeatureID { get; set; }
    public bool? isPresenting { get; set; }
    public bool? completedPresentation { get; set; }
}
