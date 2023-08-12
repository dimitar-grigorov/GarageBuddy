namespace GarageBuddy.Data.Common.Repositories
{
    using System.Linq;

    using GarageBuddy.Data.Common.Models;

    public interface IDeletableEntityRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
