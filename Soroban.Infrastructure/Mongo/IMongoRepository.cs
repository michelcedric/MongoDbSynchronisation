using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elia.Soroban.Infrastructure.Mongo
{

    /// <summary>
    /// Definition of the Mongodb repository to manage your entities
    /// </summary>
    /// <typeparam name="TEntity">The type contained in the repository</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id</typeparam>
    public interface IMongoRepository<TEntity, in TKey> where TEntity : IEntity
    {
        /// <summary>
        /// Returns the entity <typeparamref name="TEntity"/> by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve</param>
        /// <returns>The entity <typeparamref name="TEntity"/></returns>
        Task<TEntity> GetByIdAsync(TKey id);

        /// <summary>
        /// Adds the entity in the repository
        /// </summary>
        /// <param name="entity">The entity <typeparamref name="TEntity"/> to save</param>
        /// <returns></returns>
        Task<TEntity> SaveAsync(TEntity entity);

        /// <summary>
        /// Delete the entity in the repository
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity</param>
        /// <returns>The entity <typeparamref name="TEntity"/> saved</returns>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Retrieve a list of Entity <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="predicate">The expression</param>
        /// <returns>A list of your entities</returns>
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Retrieve a list of Entity <typeparamref name="TEntity"/>
        /// </summary>    
        /// <returns>A list of your entities</returns>
        Task<IEnumerable<TEntity>> FindAllAsync();

        /// <summary>
        /// Retrieve a list of Entity <typeparamref name="TEntity"/>
        /// </summary>    
        /// <returns>A list of your entities</returns>
        IEnumerable<TEntity> FindAll();

        /// <summary>
        /// Save an entiy in the repository
        /// </summary>
        /// <param name="entity">The entity <typeparamref name="TEntity"/> to save</param>
        /// <returns>The entity <typeparamref name="TEntity"/> saved</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Save a list of entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task SaveAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert a list of entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Retrieve a list of Entity : <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="predicate">The expression</param>
        /// <returns>A list of your entities</returns>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find the first element that match the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations)
        /// </summary>
        IMongoCollection<TEntity> Collection { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// <see cref="IMongoRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">The type contained in the repository</typeparam>
    public interface IMongoRepository<TEntity> : IMongoRepository<TEntity, Guid> where TEntity : IEntity
    {

    }
}