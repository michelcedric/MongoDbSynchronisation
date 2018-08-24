using MongoDB.Driver;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Elia.Soroban.Infrastructure.Mongo
{
    /// <summary>
    /// Reprensent the mongodb contect
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoDataContext
    {
        /// <summary>
        /// Initialize the context
        /// </summary>
        public MongoDataContext() : this("MongoDb")
        {
        }
        private MongoDataContext(string connectionName)
        {
            //var url = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            var url = "mongodb://localhost:27017/performance";

            var mongoUrl = new MongoUrl(url);
            IMongoClient client = new MongoClient(mongoUrl);
            MongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Get the Mongo database
        /// </summary>
        public IMongoDatabase MongoDatabase { get; }
    }
}