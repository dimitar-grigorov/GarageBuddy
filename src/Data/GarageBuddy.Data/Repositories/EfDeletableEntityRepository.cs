namespace GarageBuddy.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

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

        public IQueryable<TEntity> All(bool asReadonly, bool includeDeleted)
        {
            var query = base.All(asReadonly);

            if (!includeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool asReadonly, bool includeDeleted)
        {
            var query = base.All(search, asReadonly);

            if (!includeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }

            return query;
        }

        public override IQueryable<TEntity> All(bool asReadonly)
        {
            return base.All(asReadonly).Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search, bool asReadonly = false)
        {
            return this.All(asReadonly).Where(search);
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
