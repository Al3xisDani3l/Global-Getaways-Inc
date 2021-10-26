using GG.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace GG.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity :  class, IEntity, new()
    {

        protected internal readonly DbContext _context;

        protected internal DbSet<TEntity> _entities;

        protected internal readonly bool _isPlural;

        internal string _nameT;


        public RepositoryBase(GG.Database.GGContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
            _nameT = new TEntity().GetType().Name;
            _isPlural = _nameT.EndsWith("s");





        }

        public async Task<IDbContextTransaction> BeginTransactionAsyc()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public TEntity Create(TEntity entity)
        {
            var result = _entities.Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var result = await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;

        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            await Task.Run(() => { _entities.Remove(entity); });
            return await _context.SaveChangesAsync() > 0;

        }

        public TEntity Find(Expression<Func<TEntity, bool>> expression)
        {

            return _entities.FirstOrDefault(expression);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = _entities.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(expression);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _entities.FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, params string[] includes)
        {
            var query = _entities.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var result = await query.Where(expression).FirstOrDefaultAsync();

            return result;
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = _entities.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            var result = await query.Where(expression).FirstOrDefaultAsync();

            return result;
        }

        public ICollection<TEntity> FindMany(Expression<Func<TEntity, bool>> expression)
        {
            return _entities.Where(expression).ToList();
        }

        public async Task<ICollection<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _entities.Where(expression).ToListAsync();
        }

        public ICollection<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<ICollection<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            var result = await _entities.Select(selector).ToListAsync();
            return result;
        }

        public TEntity Update(TEntity entity)
        {
            DetachLocal(entity, entity.Id);
            _context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {


            if (!IsTracked(entity))
            {
                DetachLocal(entity, entity.Id);
                await _context.SaveChangesAsync();
            }
            else
            {
                _entities.Update(entity);
                await _context.SaveChangesAsync();
            }
           
            
            return await Task.FromResult(entity);
        }

        public bool IsTracked(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DetachLocal(TEntity entity,int id)
        {
            var detachedEntity = _entities.FirstOrDefault(en => en.Id == id);

            if (detachedEntity != null)
            {
                _context.Entry<TEntity>(detachedEntity).State = EntityState.Detached;
            }

            _context.Entry<TEntity>(entity).State = EntityState.Modified;

        }


       
        public Task<TEntity> FindAndSelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> FindManyAndSelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        bool IRepository<TEntity>.Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity FindAndSelect<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> FindManyAndSelect<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }




        #region Anulado
        //public virtual async Task<bool> Delete(int id)
        //{

        //    var currentPost = await Get(id);

        //    GetProperty(_isPlural).Remove(currentPost);

        //    int rows = await _context.SaveChangesAsync();

        //    return rows > 0;
        //}

        //public virtual async Task<IEnumerable<TEntity>> Get()
        //{

        //    return await GetProperty(_isPlural).ToListAsync();

        //}

        //public virtual async Task<TEntity> Get(int id)
        //{
        //    //return await GetProperty().FirstAsync<T>(o => (o.GetType().GetProperty("Id").GetValue(o) as int?) == id);
        //    //return await GetProperty().FirstOrDefaultAsync<T>(o => ((int)o.GetType().GetRuntimeProperty("Id").GetValue(o)) == id);
        //    return await GetProperty(_isPlural).FindAsync(id);
        //}

        //public virtual Task Patch(TEntity post)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual async Task<bool> Post(TEntity post)
        //{
        //    await GetProperty(_isPlural).AddAsync(post);
        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public virtual async Task<bool> Put(TEntity post)
        //{
        //    var currentPost = await Get((int)post.GetType().GetProperty("Id").GetValue(post));
        //    await UpdateProperty(currentPost, post);
        //    return await _context.SaveChangesAsync() > 0;


        //}

        //protected virtual async Task<TEntity> UpdateProperty(TEntity current, TEntity uptadeEntity)
        //{


        //    var properties = current.GetType().GetProperties();

        //    foreach (var item in properties)
        //    {
        //        string name = item.Name;

        //        if (name != "Id")
        //        {
        //            try
        //            {
        //                item.SetValue(current, item.GetValue(uptadeEntity));
        //            }
        //            catch (Exception non)
        //            {


        //            }



        //        }

        //    }

        //    return await Task.FromResult<TEntity>(current);






        //}

        //private DbSet<TEntity> GetProperty(bool IsLetterSnesesary = false)
        //{

        //    var _contextType = _context.GetType();

        //    PropertyInfo _propertyInfo;

        //    if (!IsLetterSnesesary)
        //    {
        //        _propertyInfo = _contextType.GetProperty(_nameT + "s");


        //    }
        //    else
        //    {
        //        _propertyInfo = _contextType.GetProperty(_nameT);
        //    }

        //    var _value = ((DbSet<TEntity>)_propertyInfo.GetValue(_context)).AsQueryable(); ;

        //    foreach (var item in typeof(TEntity).GetProperties())
        //    {
        //        if (item.GetGetMethod().IsVirtual)
        //        {
        //            _value.Include(item.Name);
        //        }

        //    }

        //    return (_value as DbSet<TEntity>);
        //}
        #endregion


    }
}
