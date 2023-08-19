namespace GarageBuddy.Tests.Common
{
    using Data.Common.Models;
    using Data.Common.Repositories;

    using GarageBuddy.Common.Core.Enums;

    using Microsoft.EntityFrameworkCore;

    using MockQueryable.Moq;

    using Moq;

    using static GarageBuddy.Common.Constants.MessageConstants;

    /// <summary>
    /// This class is a mock of <see cref="IDeletableEntityRepository{TEntity,TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key of the entity.</typeparam>
    public static class DeletableEntityRepositoryMock<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        private static readonly ICollection<TEntity> Entities = new List<TEntity>();

        /// <summary>
        /// Gets the <see cref="DeletableEntityRepositoryMock{TEntity, TKey}"/> instance of the mock.
        /// </summary>
        public static IDeletableEntityRepository<TEntity, TKey> Instance
        {
            get
            {
                var mock = new Mock<IDeletableEntityRepository<TEntity, TKey>>();
                var entitiesMock = Entities.BuildMock();

                mock.Setup(m => m.All(It.IsAny<ReadOnlyOption>(), It.IsAny<DeletedFilter>()))
                    .Returns((ReadOnlyOption asReadOnly, DeletedFilter includeDeleted) =>
                    {
                        IQueryable<TEntity> query = entitiesMock;

                        if (asReadOnly.AsBoolean())
                        {
                            query = query.AsNoTracking();
                        }

                        if (includeDeleted == DeletedFilter.NotDeleted)
                        {
                            query = query.Where(e => !e.IsDeleted);
                        }

                        return query;
                    });

                mock.Setup(m => m.All(It.IsAny<ReadOnlyOption>()))
                    .Returns((ReadOnlyOption asReadOnly) =>
                    {
                        IQueryable<TEntity> query = entitiesMock;

                        if (asReadOnly.AsBoolean())
                        {
                            query = query.AsNoTracking();
                        }

                        return query;
                    });

                mock.Setup(m => m.AddAsync(It.IsAny<TEntity>()).Result)
                    .Returns((TEntity entity) =>
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                        Entities.Add(entity);

                        return null!;
                    });

                mock.Setup(m => m.AddRangeAsync(It.IsAny<IEnumerable<TEntity>>()))
                    .Callback((IEnumerable<TEntity> entities) =>
                    {
                        foreach (var entity in entities)
                        {
                            Entities.Add(entity);
                        }
                    });

                mock.Setup(m => m.Add(It.IsAny<TEntity>()))
                    .Returns((TEntity entity) =>
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                        Entities.Add(entity);

                        return null!;
                    });

                mock.Setup(m => m.FindAsync(It.IsAny<TKey>(), It.IsAny<ReadOnlyOption>()).Result)
                    .Returns((TKey id, ReadOnlyOption asReadOnly) =>
                    {
                        ArgumentNullException.ThrowIfNull(id);

                        TEntity? entity = Entities.FirstOrDefault(e => EqualityComparer<TKey>.Default.Equals(e.Id, id));

                        if (entity == null)
                        {
                            throw new InvalidOperationException(string.Format(Errors.NoEntityWithPropertyFound, "entity", nameof(id)));
                        }

                        return entity;
                    });

                mock.Setup(m => m.ExistsAsync(It.IsAny<TKey>()).Result)
                    .Returns((TKey id) =>
                    {
                        ArgumentNullException.ThrowIfNull(id);

                        return Entities.Any(e => EqualityComparer<TKey>.Default.Equals(e.Id, id));
                    });

                mock.Setup(m => m.DeleteAsync(It.IsAny<TKey>()))
                    .Returns((TKey id) =>
                    {
                        var entity = Entities.FirstOrDefault(e => EqualityComparer<TKey>.Default.Equals(e.Id, id));

                        if (entity != null)
                        {
                            entity.IsDeleted = true;
                        }

                        return Task.CompletedTask;
                    });

                mock.Setup(m => m.Dispose()).Callback(() => Entities.Clear());

                var repository = mock.Object;
                return repository;
            }
        }

        /// <summary>
        /// This method clears the internal entities collection.
        /// </summary>
        public static void Dispose()
        {
            Entities.Clear();
        }
    }
}
