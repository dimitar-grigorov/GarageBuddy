namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Common.Models;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public interface IRepository<TEntity, in TKey> : IDisposable
        where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllReadonly();

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search);

        IQueryable<TEntity> AllReadonly(Expression<Func<TEntity, bool>> search);

        TEntity Find(TKey id, bool isReadonly = false);

        Task<TEntity> FindAsync(TKey id, bool isReadonly = false);

        EntityEntry<TEntity> Add(TEntity entity);

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        Task UpdateAsync(TKey id);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        Task DeleteAsync(TKey id);

        void DeleteRange(IEnumerable<TEntity> entities);

        EntityEntry<TEntity> Detach(TEntity entity);

        Task<int> SaveChangesAsync();

        public Task Truncate(string table);
    }
}
