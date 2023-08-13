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

        public virtual async Task<ICollection<TModel>> GetAllAsync<TModel>(QueryOptions<TModel>? queryOptions = null)
        {
            var query = this.entityRepository
                .All(queryOptions?.IsReadOnly ?? false, queryOptions?.WithDeleted ?? false)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider);

            query = this.ModifyQuery(query, queryOptions ?? new());

            var modelList = await query.ToListAsync();

            return modelList;
        }

        public virtual async Task<TModel> GetAsync<TModel>(TKey id, QueryOptions<TModel>? queryOptions = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entity = await this.entityRepository.FindAsync(id);

            var model = this.mapper.Map<TModel>(entity);

            return model;
        }

        public virtual async Task<TKey> CreateAsync<TModel>(TModel model)
        {
            var isValid = this.ValidateModel(model);

            if (!isValid)
            {
                throw new ArgumentException(
                    string.Format(Errors.EntitysModelStateIsNotValid, "Entity"),
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
                    string.Format(Errors.EntitysModelStateIsNotValid, "Entity"),
                    nameof(model));
            }

            var oldEntity = await this.entityRepository.FindAsync(id, false);

            this.CopyProperties(model, oldEntity);
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
                var entity = await this.entityRepository.FindAsync(id);
                bool withDeleted = queryOptions?.WithDeleted ?? false;

                if (withDeleted == false && entity.IsDeleted == true)
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

        protected IQueryable<TModel> ModifyQuery<TModel>(IQueryable<TModel> query, QueryOptions<TModel>? queryOptions)
        {
            if (queryOptions == null)
            {
                return query;
            }

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

        protected bool ValidateModel<TModel>(TModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, validationResults, true);

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
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true)!.IsPrivate)
                          && (targetProperty.GetSetMethod()!.Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };

            // Map the properties
            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
