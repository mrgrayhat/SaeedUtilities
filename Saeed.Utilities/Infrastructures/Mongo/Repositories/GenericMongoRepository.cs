using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Saeed.Utilities.Contracts.Domain;

using MongoDB.Bson;

using MongoDB.Driver;

using Saeed.Utilities.DynamicSettings.Database;
using Saeed.Utilities.Infrastructures.Mongo.Utils;

namespace Saeed.Utilities.Infrastructures.Mongo.Repositories
{
    public class GenericMongoRepository<TEntity> : IGenericMongoRepository<TEntity> where TEntity : IBaseEntity<string>
    {
        public IMongoCollection<TEntity> Collection { get; }
        public IMongoDatabase Database { get; }


        public GenericMongoRepository(DatabaseSettings settings)
        {
            if (settings is null || settings.ConnectionStrings.MongoDb is null)
            {
                throw new ArgumentNullException(nameof(settings), "mongo db config or connection is not set correctrly!");
            }

            Database = new MongoClient(settings.ConnectionStrings.MongoDb).GetDatabase(settings.DatabaseName);
            Collection = Database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((CollectionName)documentType.GetCustomAttributes(typeof(CollectionName), true)
                .FirstOrDefault())?.Name;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        public virtual IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return Collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return Collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => Collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(string id)
        {
            var objectId = new ObjectId(id);
            //var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return Collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                //var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
                return Collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TEntity document)
        {
            Collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TEntity document)
        {
            return Task.Run(() => Collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TEntity> documents)
        {
            Collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TEntity> documents)
        {
            await Collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            Collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            await Collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            Collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => Collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            //FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            Collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                //var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                var filter = Builders<TEntity>.Filter.Eq("_id", objectId);

                Collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filterExpression)
        {
            Collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => Collection.DeleteManyAsync(filterExpression));
        }

        public virtual Task<long> CountAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return Collection.Find(_ => true).CountDocumentsAsync();
            }
            else
            {
                var filter = Builders<TEntity>.Filter.Eq("IsDeleted", false);
                return Collection.Find(filter).CountDocumentsAsync();
            }

        }
    }
}
