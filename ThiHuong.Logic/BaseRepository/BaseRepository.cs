using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThiHuong.Framework.Models;

namespace ThiHuong.Logic.BaseRepository
{

    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void AddRangeAsync(params object[] entities);
        void AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity, TEntity updatedEntity);
        void Update(TEntity entity);
        void UpdateRange(params object[] entities);
        Task<TEntity> FindAsync(object Id);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

    }

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected ThiHuongDbContext dbContext { get; set; }

        public BaseRepository(ThiHuongDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            return query;
        }

        public void AddAsync(TEntity entity)
        {
            dbContext.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void Update(TEntity entity, TEntity updatedEntity)
        {
            var attachedEntry = dbContext.Entry(entity);

            attachedEntry.CurrentValues.SetValues(updatedEntity);
        }

        public virtual async Task<TEntity> FindAsync(object Id)
        {
            return await dbContext.Set<TEntity>().FindAsync(Id);
        }

        public virtual async void AddRangeAsync(params object[] entities)
        {
            await dbContext.AddRangeAsync(entities);
        }

        public virtual void UpdateRange(params object[] entities)
        {
            dbContext.UpdateRange(entities);
        }

        public void Update(TEntity entityToUpdate)
        {
            dbContext.Set<TEntity>().Attach(entityToUpdate);
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
