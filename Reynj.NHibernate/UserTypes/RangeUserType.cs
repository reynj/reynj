using System;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace Reynj.NHibernate.UserTypes
{
    /// <summary>
    /// Base class for ImmutableUserTypes
    /// </summary>
    public class RangeUserType<T> : ICompositeUserType
        where T : IComparable
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly IType PropertyType = GetNHibernateType();

        /// <inheritdoc />
        public string[] PropertyNames => new[] {"Start", "End"};

        /// <inheritdoc />
        public IType[] PropertyTypes => new[] {PropertyType, PropertyType};

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="property"/> is not 0 or 1.</exception>
        public object GetPropertyValue(object component, int property)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var range = (Range<T>) component;

            return property switch
            {
                0 => range.Start,
                1 => range.End,
                _ => throw new ArgumentOutOfRangeException(nameof(property), property,
                    $"The parameter {nameof(property)} must have the value 0 or 1.")
            };
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">If this method gets called.</exception>
        public void SetPropertyValue(object component, int property, object value)
        {
            throw new NotSupportedException("SetPropertyValue is not supported on immutable types.");
        }

        /// <inheritdoc />
        public Type ReturnedClass => typeof(Range<T>);

        /// <inheritdoc />
        public new bool Equals(object? x, object? y) => object.Equals(x, y);

        /// <inheritdoc />
        public int GetHashCode(object? x) => x?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public object? NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void NullSafeSet(DbCommand cmd, object? value, int index, bool[] settable, ISessionImplementor session)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object? DeepCopy(object? value)
        {
            var original = value as Range<T>;
            return original?.Clone();
        }

        /// <inheritdoc />
        public bool IsMutable => false;

        /// <inheritdoc />
        public object? Disassemble(object? value, ISessionImplementor session)
        {
            return DeepCopy(value);
        }

        /// <inheritdoc />
        public object? Assemble(object? cached, ISessionImplementor session, object? owner)
        {
            return DeepCopy(cached);
        }

        /// <inheritdoc />
        public object? Replace(object? original, object? target, ISessionImplementor session, object? owner)
        {
            return DeepCopy(original);
        }

        private static IType GetNHibernateType()
        {
            if (typeof(T) == typeof(string))
                return NHibernateUtil.String;

            var instance = Activator.CreateInstance<T>();
            return NHibernateUtil.GuessType(instance);
        }
    }
}