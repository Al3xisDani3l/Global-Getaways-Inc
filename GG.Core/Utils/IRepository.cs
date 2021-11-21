using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GG.Core
{
    public interface IRepository<T,Tkey> where T : class, IEntity<Tkey>, new() where Tkey : IEquatable<Tkey>
    {

        Task<T> UpdateAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression, params string[] includes);
        Task<T> FindAndSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> where);
        Task<ICollection<T>> FindManyAsync(Expression<Func<T, bool>> expression);
        Task<ICollection<T>> FindManyAndSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> where);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector);

        Task<T> FindByKeyAsync(Tkey tkey);
        void DetachLocal(T entity, Tkey id);


        T Update(T entity);
        T Create(T entity);
        bool Delete(T entity);
        T Find(Expression<Func<T, bool>> expression);
        T Find(Expression<Func<T, bool>> expression, params string[] includes);
        Task<T> FindAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        T FindAndSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> where);
        ICollection<T> FindMany(Expression<Func<T, bool>> expression);
        ICollection<T> FindManyAndSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> where);
        ICollection<T> GetAll();
        Task<int> CommitChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsyc();
    }
}

