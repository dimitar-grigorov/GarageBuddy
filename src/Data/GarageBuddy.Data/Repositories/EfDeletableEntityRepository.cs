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

        public override IQueryable<TEntity> All()
        {
            return base.All().Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> All(Expression<Func<TEntity, bool>> search)
        {
            // TODO: Check if this works
            return this.All().Where(search).Where(x => !x.IsDeleted);
        }

        public override IQueryable<TEntity> AllReadonly()
        {
            return base.AllReadonly().Where(x => !x.IsDeleted);
        }

        public IQueryable<TEntity> AllWithDeleted()
        {
            return base.All().IgnoreQueryFilters();
        }

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted()
        {
            return base.AllReadonly().IgnoreQueryFilters();
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
