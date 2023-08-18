namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Enums;

    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using Models;

    public interface IRepository<TEntity, in TKey> : IDisposable
        where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> All(ReadOnlyOption asReadOnly);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, ReadOnlyOption asReadOnly);

        Task<TEntity> FindAsync(TKey id, ReadOnlyOption asReadOnly);

        Task<bool> ExistsAsync(TKey id);

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
