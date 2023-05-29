using FasenmyerConference.Models;
using Microsoft.Extensions.Options;

namespace FasenmyerConference.Services.AzureMapsServices
{
    public class AzureMapsService : BackgroundService
    {
        DateTime nextOperationalDay;

        // Will log Azure map updates to the terminal
        private readonly ILogger<AzureMapsService> _azureLogger;
        private readonly IOptions<AzureConfig> _AzureConfigAtt;
        private AzureServiceMediator _AzureMediator;

        // Constructor
        public AzureMapsService(ILogger<AzureMapsService> inputlogger, IOptions<AzureConfig> AzureServiceConfig)
        {
            _AzureConfigAtt = AzureServiceConfig;
            _azureLogger = inputlogger;

        }

        // The method that starts the automated periodic operation
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This object handles all the services
            _AzureMediator = new AzureServiceMediator();

            // Loading config variables into objects
            _AzureMediator.loadAzureDBConfig(_AzureConfigAtt.Value.FasenmyerConferenceContext);
            _AzureMediator.loadAzureAPIConfig(_AzureConfigAtt.Value.AzureSubscriptionKey, _AzureConfigAtt.Value.statesetId, _AzureConfigAtt.Value.datasetId);

            // Wait until the mediator starts and set-up all the service helper classes
            await _AzureMediator.AzureServiceInitAsync();

            // Calculates the next day
            nextOperationalDay = DateTime.Parse(_AzureMediator.getOperationalDate()).AddDays(1);

            // Periodic checking of database and updating Azure Indoor map every 30 minutes
            while (!stoppingToken.IsCancellationRequested)
            {
                // This conditional grabs the presentations for today and will loop until all presentations are done
                if ((_AzureMediator.getOperationalDate() == DateTime.Now.Date.ToShortDateString()) && (!_AzureMediator.getIsDone()) && (_AzureMediator.getSubscriberCount() > 0))
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                    //await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                    _azureLogger.LogInformation("Azure Map Service async execution and map updating: {datetime}", DateTime.Now);

                    // Checking and updating if the presentations are done
                    _ = _AzureMediator.PresentationUpdateAsync(false);

                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                    // Checking and updating the presentations are starting
                    _ = _AzureMediator.PresentationUpdateAsync(true);

                }
                else if ((nextOperationalDay.ToShortDateString() == DateTime.Now.Date.ToShortDateString()))
                {
                    // Once a new day starts, the mediator is reloaded and the presentations for today are prepped
                    _azureLogger.LogInformation("Reloading Azure Map Service: {datetime}", DateTime.Now);
                    await _AzureMediator.AzureServiceInitAsync();
                    nextOperationalDay = DateTime.Parse(_AzureMediator.getOperationalDate()).AddDays(1);

                }
                else
                {
                    // Once all the presentations are done, the Azure Service checks once an hour to see if it's the next day
                    _azureLogger.LogInformation("Azure Map Service Paused for an hour due to completed operations: {datetime}", DateTime.Now);
                    await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
                }

            }

        }

        // Ending aync background service when system closes
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _azureLogger.LogInformation("Azure Map Service - stopping async: {datetime}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
