using MongoDB.Driver;
using TroubleTrack.Controllers;

namespace TroubleTrack.Database
{
    public class MongoDatabase
    {
        public MongoClient DBclient;
        private readonly ILogger<ProjectsController> _logger;
        public bool Connected => DBclient != null;

        public MongoDatabase(ILogger<ProjectsController> logger) 
        {
            _logger = logger;
            ConnectToDB();
        }
        private async void ConnectToDB()
        {
            MongoClient client = new MongoClient();
            int retryDelay = 5; //in seconds
            bool connected = false;
            while (!connected)
            {
                _logger.LogInformation("Connecting to database");
                try
                {
                    MongoClientSettings settings = null;
                    await Task.Run(new Action(() =>
                    {
                        settings = MongoClientSettings.FromConnectionString(connectionString: DbConfig.DbConnectionString);
                        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                        settings.ConnectTimeout = TimeSpan.FromSeconds(1);
                        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(1);
                        settings.RetryWrites = true;
                        settings.RetryReads = true;
                    }));

                    client = new MongoClient(settings);
                    await client.ListDatabaseNamesAsync();
                    connected = true;
                    _logger.LogInformation($"Successfully connected to database");

                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"{ex.GetType().Name.ToLower()}");
                    for (int i = retryDelay; i > 0; i--)
                    {
                        _logger.LogError($"Connection failed, retrying in {i} seconds");
                        await Task.Delay(1 * 1000); //convert to miliseconds
                    }

                }
            }
            DBclient =  client;
        }
    }
}
