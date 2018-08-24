using MongoDB.Driver;
using System;
using System.Configuration;

namespace MongoSync.MongoDb
{
    /// <summary>
    /// Manage the context to connect on mongo (connection to source and target)
    /// </summary>
    public class MongoDbDataContext
    {
        /// <summary>
        /// Get the source Mongo database
        /// </summary>
        public IMongoDatabase SourceMongoDbDatabase { get; private set; }

        /// <summary>
        /// Get the target Mongo database
        /// </summary>
        public IMongoDatabase TargetMongoDbDatabase { get; private set; }

        /// <summary>
        /// Initialize the context with connection string from config file
        /// </summary>
        public MongoDbDataContext()
        {
            InitializeMongoDbDataContextFromConfig("MongoSourceUrl", "MongoTargetUrl");
        }

        /// <summary>
        /// Initialize the context with a specific source url and target url
        /// </summary>
        /// <param name="connectionStringSourceUrl"></param>
        /// <param name="connectionStringTargetUrl"></param>
        public MongoDbDataContext(string connectionStringSourceUrl, string connectionStringTargetUrl)
        {
            InitializeMongoDbDataContextFromUrl(connectionStringSourceUrl, connectionStringTargetUrl);
        }

        private void InitializeMongoDbDataContextFromConfig(string sourceConnectionName, string targetConnectionName)
        {
            var sourceUrl = ConfigurationManager.ConnectionStrings[sourceConnectionName].ConnectionString;
            var targetUrl = ConfigurationManager.ConnectionStrings[targetConnectionName].ConnectionString;

            InitializeMongoDbDataContextFromUrl(sourceUrl, targetUrl);
        }

        private void InitializeMongoDbDataContextFromUrl(string sourceUrl, string targetUrl)
        {
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Source Url : {sourceUrl}");
            Console.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff")} - Target Url : {targetUrl}");
            SourceMongoDbDatabase = GetMongoDbDatabase(sourceUrl);
            TargetMongoDbDatabase = GetMongoDbDatabase(targetUrl);
        }

        private IMongoDatabase GetMongoDbDatabase(string url)
        {
            var mongoUrl = new MongoUrl(url);
            IMongoClient client = new MongoClient(mongoUrl);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}
