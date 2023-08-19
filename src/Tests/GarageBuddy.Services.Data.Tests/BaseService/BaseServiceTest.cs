
namespace GarageBuddy.Services.Data.Tests.BaseService
{
    using AutoMapper;

    using Common;

    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Common.Repositories;

    public class BaseServiceTest<TEntity, TKey> : BaseService<TEntity, TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        public BaseServiceTest(IDeletableEntityRepository<TEntity, TKey> entityRepository, IMapper mapper)
            : base(entityRepository, mapper)
        {
        }

        public new bool ValidateModel<TModel>(TModel model)
        {
            return base.ValidateModel(model);
        }
    }
}
