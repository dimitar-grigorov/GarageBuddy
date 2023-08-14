namespace GarageBuddy.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Common;

    using GarageBuddy.Common.Core.Wrapper.Generic;

    public interface IBaseService<TKey>
    {
        public Task<ICollection<TModel>> GetAllAsync<TModel>(bool asReadOnly = false, bool includeDeleted = false);

        public Task<PaginatedResult<TModel>> GetAllAsync<TModel>(QueryOptions<TModel> queryOptions);

        public Task<TModel> GetAsync<TModel>(TKey id);

        public Task<TKey> CreateAsync<TModel>(TModel model);

        public Task EditAsync<TModel>(TKey id, TModel model);

        public Task DeleteAsync<TModel>(TKey id);

        public Task<bool> ExistsAsync<TModel>(TKey id, QueryOptions<TModel>? queryOptions = null);
    }
}
