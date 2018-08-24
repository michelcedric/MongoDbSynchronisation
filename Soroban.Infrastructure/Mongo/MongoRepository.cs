using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elia.Soroban.Infrastructure.Mongo
{
    /// <inheritdoc cref="IMongoRepository{TEntity,TKey}" />
    /// <summary>
    /// <see cref="IMongoRepository{TEntity,TKey}"/>
    /// </summary>
    /// <typeparam name="TEntity">The type contained in the repository</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class MongoRepository<TEntity> : MongoRepositoryBase<TEntity>, IMongoRepository<TEntity> where TEntity : IEntity
    {

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.GetByIdAsync"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            // return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            // Performance issue : https://stackoverflow.com/questions/45454750/mongo-c-sharp-where-is-findone
            return await Collection.Find(x => x.Id.Equals(id)).Limit(1).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.SaveAsync(TEntity)"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            return entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.SaveAsync(TEntity)"/>
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task SaveAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                await SaveAsync(entity);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.BulkInsertAsync"/>
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            await Collection.InsertManyAsync(entities);
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.Save"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Save(TEntity entity)
        {
            Collection.ReplaceOne(x => x.Id.Equals(entity.Id), entity, new UpdateOptions { IsUpsert = true });

            return entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.FindAll()"/>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate).ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.DeleteAsync"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(Guid id)
        {
            await Collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.DeleteAsync"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void Delete(Guid id)
        {
            Collection.DeleteOne(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.FindAllAsync(Expression{Func{TEntity,bool}})"/>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.FindAllAsync()"/>
        /// </summary>   
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await Collection.Find(new BsonDocument()).ToListAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.FindAll()"/>
        /// </summary>   
        /// <returns></returns>
        public virtual IEnumerable<TEntity> FindAll()
        {
            return Collection.Find(new BsonDocument()).ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IMongoRepository{TEntity,TKey}.FirstAsync(Expression{Func{TEntity,bool}})"/>
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            // return await Collection.Find(predicate).FirstOrDefaultAsync();
            // Performance issue : https://stackoverflow.com/questions/45454750/mongo-c-sharp-where-is-findone
            return await Collection.Find(predicate).Limit(1).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> Exist<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            var expression = (MemberExpression)field.Body;

            FieldDefinition<TEntity, TField> fieldDefinition = expression.Member.Name;
            var filter = Builders<TEntity>.Filter.Eq(fieldDefinition, value);
            return await Collection.Find(filter).AnyAsync();
        }
    }
}