using System;
using MongoDB.Bson;

namespace Elia.Soroban.Infrastructure.Mongo
{
    /// <summary>
    /// The definition of the entity
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id</typeparam>
    public interface IEntity<out TKey>
    {
        /// <summary>
        /// The unique identifier
        /// </summary>
        TKey Id { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// The definition of the entity
    /// </summary>
    public interface IEntity : IEntity<Guid>
    {
    }
}
