namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GarageBuddy.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        IQueryable<TEntity> All(bool asReadonly, bool includeDeleted);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool asReadonly, bool includeDeleted);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
