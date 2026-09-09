using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProbashiShield.DAL.Repositories.Base
{
    public interface IGenericRepository<T, C>
        where T : class
        where C : DbContext
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        Task<bool> RemoveByIdAsync(long id);
        Task<bool> RemoveByIdAsync(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        Task<T> GetByIdAsync(decimal id);
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetWithTrack(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        Task ExecuteFromSqlRaw(string query, SqlParameter[] Parameters = null);
        IQueryable<T> GetFromSqlRaw(string query, SqlParameter[] Parameters = null);
    }
}
