namespace GarageBuddy.Services.Data.Common
{
    using System.Collections.Generic;

    using GarageBuddy.Common.Core.Enums;

    /// <summary>
    /// This class is used to modify service queries. That way we eliminate the creating of multiple similar methods.
    /// Example:
    /// <code>GetAll();
    /// GetAllOrderedByName();
    /// GetAllOrderedByNameAsNoTracking();
    /// GetAllOrderedByNameWithDeletedAsNoTracking();
    /// </code>
    /// All of this can be achieved by using like this:
    /// <code>GetAll(QueryOptions options);</code>
    /// </summary>
    /// <typeparam name="TModel">The query entity.</typeparam>
    public class QueryOptions<TModel>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entities should be tracked.
        /// </summary>
        public ReadOnlyOption AsReadOnly { get; set; } = ReadOnlyOption.Normal;

        /// <summary>
        /// Gets or sets a value indicating whether only not deleted entities should be returned.
        /// </summary>
        public DeletedFilter IncludeDeleted { get; set; } = DeletedFilter.NotDeleted;

        /// <summary>
        /// Gets or sets a list of <see cref="OrderOption{TClass}"/>.
        /// The direct use of <see cref="List{T}"/> instead of <see cref="ICollection{T}"/> or <see cref="IEnumerable{T}"/> is because
        /// the ease of use and readability of the <c>new()</c> operator.
        /// </summary>
        public List<OrderOption<TModel>> OrderOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the amount of entities to be skipped.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the amount of entities to be taken.
        /// </summary>
        public int? Take { get; set; }
    }
}
