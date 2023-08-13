namespace GarageBuddy.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Common.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class EfDeletableEntityRepository<TEntity, TKey> : EfRepository<TEntity, TKey>, IDeletableEntityRepository<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        public EfDeletableEntityRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IQueryable<TEntity> All(bool isReadonly, bool withDeleted)
        {
            var query = base.All(isReadonly);

            if (!withDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool isReadonly, bool withDeleted)
        {
            var query = base.All(search, isReadonly);

            if (!withDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public override IQueryable<TEntity> All(bool isReadonly = false)
        {
            return base.All(isReadonly).Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool isReadonly = false)
        {
            return this.All(isReadonly).Where(search);
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
