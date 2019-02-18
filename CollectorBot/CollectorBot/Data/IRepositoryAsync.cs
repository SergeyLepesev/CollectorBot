using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CollectorBot.Data {
    public interface IRepositoryAsync<T> {
        Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> filter = null);
        Task<T> GetItem(Expression<Func<T, bool>> filter);
        Task Create(T item);
        Task Update(T item, Expression<Func<T, bool>> filter);
        Task Delete(Expression<Func<T, bool>> filter);
    }
}