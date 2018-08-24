using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Elia.Soroban.Infrastructure.Mongo
{
    /// <summary>
    /// Base respository of mongo to manage the collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [ExcludeFromCodeCoverage]
    public class MongoRepositoryBase<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// <see cref="IMongoRepository{TEntity}.Collection" />
        /// </summary>
        public IMongoCollection<TEntity> Collection { get; }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        protected MongoRepositoryBase()
        {
            var settings = new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.Standard
            };

            Collection = new MongoDataContext().MongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name, settings);
            
        }

        /// <summary>
        ///  Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="collectionName">The mongo collection name in the repository</param>
        protected MongoRepositoryBase(string collectionName)
        {
            Collection = new MongoDataContext().MongoDatabase.GetCollection<TEntity>(collectionName);
        }
    }
}
