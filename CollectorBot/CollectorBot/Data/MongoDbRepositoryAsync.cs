using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CollectorBot.Data {
    public class MongoDbRepositoryAsync<T> : IRepositoryAsync<T> where  T : class {
        private readonly MongoContext _database;

        public MongoDbRepositoryAsync(MongoContext mongoContext) {
            _database = mongoContext;
        }

        public async Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> filter = null) {
            return filter is null
                   ? await _database.GetItems<T>().Find(Builders<T>.Filter.Empty).ToListAsync()
                   : await _database.GetItems<T>().Find(filter).ToListAsync();
        }

        public async Task<T> GetItem(Expression<Func<T, bool>> filter) {
            return await _database.GetItems<T>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task Create(T item) {
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