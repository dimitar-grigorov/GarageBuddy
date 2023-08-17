namespace GarageBuddy.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Common.Repositories;

    public class EfDeletableEntityRepository<TEntity, TKey> : EfRepository<TEntity, TKey>, IDeletableEntityRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        public EfDeletableEntityRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IQueryable<TEntity> All(ReadOnlyOption asReadOnly, DeletedFilter includeDeleted)
        {
            var query = base.All(asReadOnly);

            if (includeDeleted == DeletedFilter.NotDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search,
            ReadOnlyOption asReadOnly, DeletedFilter includeDeleted)
        {
            var query = base.All(search, asReadOnly);

            if (includeDeleted == DeletedFilter.NotDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public override IQueryable<TEntity> All(ReadOnlyOption asReadOnly)
        {
            return base.All(asReadOnly).Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search,
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal)
        {
            return this.All(asReadOnly).Where(search);
        }

        public void HardDelete(TEntity entity)
        {
            base.Delete(entity);
        }

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }
    }
}
