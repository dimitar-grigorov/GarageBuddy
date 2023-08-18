namespace GarageBuddy.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Common;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Common.Core.Wrapper.Generic;

    public interface IBaseService<TKey>
    {
        public Task<ICollection<TModel>> GetAllAsync<TModel>(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted);

        public Task<PaginatedResult<TModel>> GetAllAsync<TModel>(QueryOptions<TModel> queryOptions);

        public Task<TModel> GetAsync<TModel>(TKey id);

        public Task<TKey> CreateAsync<TModel>(TModel model);

        public Task DeleteAsync<TModel>(TKey id);

        public Task<bool> ExistsAsync<TModel>(TKey id, QueryOptions<TModel>? queryOptions = null);

        public Task<IResult<TKey>> EditAsync<TModel>(TKey id, TModel model, string entityName);

        public Task<IResult<TKey>> CreateBasicAsync<TModel>(TModel model, string entityName);
    }
}
