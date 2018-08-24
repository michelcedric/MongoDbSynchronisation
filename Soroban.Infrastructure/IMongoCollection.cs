using MongoDB.Driver;

namespace Elia.Soroban.Infrastructure
{
    public interface IMongoDbCollection<TEntity>
    {
        IMongoCollection<TEntity> Collection { get; }
    }
}
