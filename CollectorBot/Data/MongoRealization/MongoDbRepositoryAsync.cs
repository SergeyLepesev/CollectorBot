using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism;
using MongoDB.Driver;

namespace CollectorBot.Data.MongoRealization {
    public class MongoDbRepositoryAsync<T> : IRepositoryAsync<T> where  T : class {
        private readonly MongoContext _database;
        private readonly MongoConstrain _mongoConstrain;

        public MongoDbRepositoryAsync(MongoContext mongoContext) {
            _database = mongoContext;
            _mongoConstrain = new MongoConstrain(mongoContext);
        }

        public async Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> filter = null) {
            return await _database.GetItems<T>().Find(filter ?? Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> GetItem(Expression<Func<T, bool>> filter) {
            return await _database.GetItems<T>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task Create(T item) {
            _mongoConstrain.InvokeConstrainEntity(item, RepositoryMethod.Create);
            await _database.GetItems<T>().InsertOneAsync(item);
        }

        public async Task Update(T item, Expression<Func<T, bool>> filter) {
            await _database.GetItems<T>().ReplaceOneAsync(filter, item);
        }

        public async Task Delete(Expression<Func<T, bool>> filter) {
            await _database.GetItems<T>().FindOneAndDeleteAsync(filter);
        }
    }
}