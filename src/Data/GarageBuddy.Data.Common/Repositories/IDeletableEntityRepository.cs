namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        IQueryable<TEntity> All(ReadOnlyOption asReadOnly, DeletedFilter includeDeleted);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, ReadOnlyOption asReadOnly, DeletedFilter includeDeleted);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
