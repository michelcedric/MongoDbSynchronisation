using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Elia.Soroban.Infrastructure.Mongo.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Base entity
    /// <see cref="IEntity"/>
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// <see cref="IEntity.Id"/>
        /// </summary>       
        [BsonId]
        public Guid Id { get; set; }
       
        public int MyProperty { get; set; }
    }
}
