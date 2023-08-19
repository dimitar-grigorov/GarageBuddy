namespace GarageBuddy.Services.Data.Tests.Base
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Services.Data.Common;

    /// <summary>
    /// This is a test class of <see cref="BaseService{TEntity,TKey}"/> used for testing purposes.
    /// </summary>
    /// <typeparam name="TEntity">The data model entity.</typeparam>
    /// <typeparam name="TKey">The type of the id of the <typeparamref name="TEntity"/>.</typeparam>
    public class BaseServiceTest<TEntity, TKey> : BaseService<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceTest{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="entityRepository">The repository of the <typeparamref name="TEntity"/>.</param>
        /// <param name="mapper">The implementation of <see cref="IMapper"/>.</param>
        public BaseServiceTest(IDeletableEntityRepository<TEntity, TKey> entityRepository, IMapper mapper)
            : base(entityRepository, mapper)
        {
        }

        /// <summary>
        /// This method exposes the <see langword="protected"/> <see cref="BaseService{TEntity, TKey}.ValidateModel{TModel}(TModel)"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to be validated.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the model is valid.</returns>
        protected new bool ValidateModel<TModel>(TModel model)
        {
            return base.ValidateModel(model);
        }
    }
}
