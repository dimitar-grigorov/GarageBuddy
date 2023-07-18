namespace GarageBuddy.Data.Common.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GarageBuddy.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        IQueryable<TEntity> All(bool isReadonly, bool withDeleted);

        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool isReadonly, bool withDeleted);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
