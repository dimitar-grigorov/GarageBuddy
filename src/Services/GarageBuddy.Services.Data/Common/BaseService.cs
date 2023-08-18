namespace GarageBuddy.Services.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Contracts;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Common.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class BaseService<TEntity, TKey> : IBaseService<TKey>
        where TEntity : BaseDeletableModel<TKey>
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<TEntity, TKey> entityRepository;

        public BaseService(IDeletableEntityRepository<TEntity, TKey> entityRepository, IMapper mapper)
        {
            this.entityRepository = entityRepository;
            this.mapper = mapper;
        }

        protected IMapper Mapper => this.mapper;

        public async Task<ICollection<TModel>> GetAllAsync<TModel>(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted)
        {
            var query = this.entityRepository
                .All(asReadOnly, includeDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider);
            return await query.ToListAsync();
        }

        public virtual async Task<PaginatedResult<TModel>> GetAllAsync<TModel>(QueryOptions<TModel> queryOptions)
        {
            var query = this.entityRepository
                .All(queryOptions.AsReadOnly, queryOptions.IncludeDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider);
            var modelList = await ModifyQuery(query, queryOptions).ToListAsync();

            var totalCount = await GetTotalCountForPagination(queryOptions);

            return PaginatedResult<TModel>.Success(modelList, totalCount);
        }

        public virtual async Task<TModel> GetAsync<TModel>(TKey id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await this.entityRepository.FindAsync(id, ReadOnlyOption.ReadOnly);

            var model = this.mapper.Map<TModel>(entity);

            return model;
        }

        public virtual async Task<TKey> CreateAsync<TModel>(TModel model)
        {
            var isValid = this.ValidateModel(model);

            if (!isValid)
            {
                throw new ArgumentException(
                    string.Format(Errors.EntityModelStateIsNotValid, "Entity"),
                    nameof(model));
            }

            var entity = this.mapper.Map<TEntity>(model);

            var result = await this.entityRepository.AddAsync(entity);
            await this.entityRepository.SaveChangesAsync();

            return result.Entity.Id ?? default!;
        }

        public virtual async Task EditAsync<TModel>(TKey id, TModel model)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            if (!await this.ExistsAsync<TModel>(id))
            {
                throw new InvalidOperationException(string.Format(Errors.EntityNotFound, "entity"));
            }

            var isValid = this.ValidateModel(model);

            if (!isValid)
            {
                throw new ArgumentException(
                    string.Format(Errors.EntityModelStateIsNotValid, "Entity"),
                    nameof(model));
            }

            var oldEntity = await this.entityRepository.FindAsync(id, ReadOnlyOption.Normal);

            // Preserve and don't edit Created On date
            var oldCreatedOn = oldEntity.CreatedOn;
            var oldDeleted = oldEntity.IsDeleted;

            this.CopyProperties(model, oldEntity);

            // If Deleted is changed, Update DeletedOn
            if (oldEntity.IsDeleted != oldDeleted)
            {
                if (oldEntity.IsDeleted)
                {
                    oldEntity.DeletedOn = DateTime.Now;
                }
                else
                {
                    oldEntity.DeletedOn = null;
                }
            }

            oldEntity.CreatedOn = oldCreatedOn;
            oldEntity.ModifiedOn = DateTime.Now;

            await this.entityRepository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync<TModel>(TKey id)
        {
            if (!await this.ExistsAsync<TModel>(id))
            {
                throw new InvalidOperationException(string.Format(Errors.EntityNotFound, "entity"));
            }

            await this.entityRepository.DeleteAsync(id);
            await this.entityRepository.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync<TModel>(TKey id, QueryOptions<TModel>? queryOptions = null)
        {
            var result = true;

            try
            {
                var entity = await this.entityRepository.FindAsync(id, queryOptions?.AsReadOnly ?? ReadOnlyOption.Normal);
                var withDeleted = queryOptions?.IncludeDeleted ?? DeletedFilter.NotDeleted;

                if (withDeleted == DeletedFilter.Deleted && entity.IsDeleted)
                {
                    result = false;
                }
            }
            catch (InvalidOperationException)
            {
                result = false;
            }

            return result;
        }

        protected IQueryable<TModel> ModifyQuery<TModel>(IQueryable<TModel> query, QueryOptions<TModel> queryOptions)
        {
            foreach (var orderOption in queryOptions.OrderOptions)
            {
                query = orderOption.Order == OrderByOrder.Ascending
                    ? query.OrderBy(orderOption.Property)
                    : query.OrderByDescending(orderOption.Property);
            }

            if (queryOptions.Skip.HasValue)
            {
                query = query.Skip(queryOptions.Skip.Value);
            }

            if (queryOptions.Take.HasValue)
            {
                query = query.Take(queryOptions.Take.Value);
            }

            return query;
        }

        protected virtual async Task<int> GetTotalCountForPagination<TModel>(QueryOptions<TModel> queryOptions)
        {
            var totalCount = 0;
            if (queryOptions.Take.HasValue)
            {
                totalCount = await this.entityRepository
                    .All(queryOptions.AsReadOnly, queryOptions.IncludeDeleted).CountAsync();
            }

            return await Task.FromResult(totalCount);
        }

        protected bool ValidateModel<TModel>(TModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, validationResults, true);

            return isValid;
        }

#pragma warning disable IDE0060 // Remove unused parameter / The parameter is used for generic intellisense
        private PropertyInfo GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Type type = typeof(TSource);
            MemberExpression? member = propertyLambda.Body as MemberExpression;

            if (member == null)
            {
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            PropertyInfo? propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType!))
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda,
                    type));
            }

            return propInfo;
        }

        private void CopyProperties(object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
            {
                throw new Exception(Errors.SourceOrDestinationNull);
            }

            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Collect all the valid properties to map
            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && targetProperty.Name != nameof(BaseModel<TKey>.Id)
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true)!.IsPrivate)
                          && (targetProperty.GetSetMethod()!.Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty };

            // Map the properties
            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
