using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ThiHuong.Framework;
using ThiHuong.Framework.Models;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic.BaseRepository;

namespace ThiHuong.Logic.BaseService
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        Task Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity, TEntity updatedEntity);
        Task<TEntity> FindAsync(object Id);
        Task<List<TResult>> Get<TResult>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TResult : BaseViewModel;

    }

    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        protected IBaseRepository<TEntity> repository;
        protected UnitOfWork unitOfWork;

        public BaseService(IBaseRepository<TEntity> repository, UnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Add(TEntity entity)
        {
            await this.repository.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            this.repository.Delete(entity);
        }

        public async Task<TEntity> FindAsync(object Id)
        {
            return await this.repository.FindAsync(Id);
        }

        public async Task<List<TResult>> Get<TResult>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
            where TResult : BaseViewModel
        {
            var entityResult = await this.repository.Get(filter, orderBy, includeProperties)
                                                    .ToListAsync();

            return entityResult.ToListViewModel<TEntity, TResult>();
        }

        public void Update(TEntity entity, TEntity updatedEntity)
        {
            this.repository.Update(entity, updatedEntity);
        }
    }
}
