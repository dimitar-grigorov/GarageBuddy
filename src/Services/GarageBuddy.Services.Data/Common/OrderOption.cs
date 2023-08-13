namespace GarageBuddy.Services.Data.Common
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// This class encapsulates one order option of a query.
    /// It is used in <see cref="QueryOptions{TDto}"/> as a list of order options.
    /// </summary>
    /// <typeparam name="TClass">The class that should be ordered in a collection.</typeparam>
    public class OrderOption<TClass>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderOption{TClass}"/> class.
        /// </summary>
        /// <param name="propertyLambda">The lambda expression used to capture the object's property.</param>
        /// <param name="order">A parameter of type <see cref="OrderByOrder"/>.</param>
        /// <exception cref="ArgumentNullException">Throws when any of the parameters is null.</exception>
        public OrderOption(Expression<Func<TClass, object>> propertyLambda, OrderByOrder order)
        {
            this.Property = propertyLambda ?? throw new ArgumentNullException(nameof(propertyLambda));
            this.Order = order;
        }

        /// <summary>
        /// Gets or sets he lambda expression used to capture the object's property.
        /// </summary>
        public Expression<Func<TClass, object>> Property { get; set; }

        /// <summary>
        /// Gets or sets an enumeral of type <see cref="OrderByOrder"/>.
        /// </summary>
        public OrderByOrder Order { get; set; }
    }
}
