using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elia.Soroban.Infrastructure.Mongo.Models
{
    /// <summary>
    /// Base entity for the nested object
    /// </summary>
    public class NestedEntity
    {
        /// <summary>
        /// <see cref="IEntity.Id"/>
        /// </summary>       
        [BsonId]
        public Guid Id { get; protected set; }

    }
}
