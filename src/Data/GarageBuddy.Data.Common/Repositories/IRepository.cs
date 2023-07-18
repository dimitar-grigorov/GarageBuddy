namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Common.Models;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    // TODO: Consider making TEntity more generic
    // TODO: Remove not async methods
    // TODO: Check if we need to use EntityEntry<TEntity> or just TEntity
    // TODO: Add AsNoTracking
    public interface IRepository<TEntity, in TKey> : IDisposable
        where TEntity : BaseModel<TKey>
    {

        IQueryable<TEntity> All(bool isReadonly = false);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool isReadonly = false);

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
    }
}
