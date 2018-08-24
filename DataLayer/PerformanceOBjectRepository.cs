using Domain;
using Elia.Soroban.Infrastructure;
using Elia.Soroban.Infrastructure.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PerformanceOBjectRepository : MongoRepository<PerformanceRootObject>, IMongoDbCollection<PerformanceRootObject>
    {
        public async Task AddChildAsync(PerformanceChildObject child, Guid rootId)
        {
            await Collection.UpdateOneAsync(c => c.Id == rootId, Builders<PerformanceRootObject>.Update.Push(r => r.Childs, child));
        }

        public async Task AddChildsAsync(IEnumerable<PerformanceChildObject> childs, Guid rootId)
        {
            await Collection.UpdateOneAsync(c => c.Id == rootId, Builders<PerformanceRootObject>.Update.PushEach(r => r.Childs, childs));
        }

        public async Task<bool> Exist(string myProperty1, Guid rootId)
        {
            var filter = Builders<PerformanceRootObject>.Filter.ElemMatch(c => c.Childs, c => c.MyProperty1 == myProperty1);
            IAsyncCursor<PerformanceRootObject> element = await Collection.FindAsync(filter);
            return await element.AnyAsync();
        }

        public async Task<bool> Exist(IEnumerable<string> myProperty1List, Guid rootId)
        {
            var filter = FilterDefinition<PerformanceRootObject>.Empty;
            foreach (string item in myProperty1List)
            {
                filter = filter & Builders<PerformanceRootObject>.Filter.ElemMatch(c => c.Childs, c => c.MyProperty1 == item);
            }

            IAsyncCursor<PerformanceRootObject> element = await Collection.FindAsync(filter);
            return element.Any();
        }
    }
}
