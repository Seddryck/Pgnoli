using Pgnoli.Options.DateStyles;
using Pgnoli.Types.TypeHandlers.Text;
using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class TextTypeHandlerFactory : ITypeHandlerFactory
    {
        private IDateStyle DateStyle { get; }

        public TextTypeHandlerFactory(IDateStyle dateStyle)
            => DateStyle = dateStyle;

        public virtual ITypeHandler Instantiate<T>()
            => Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.String => new StringTypeHandler(),
                TypeCode.Char => new CharTypeHandler(),
                TypeCode.Boolean => new BooleanTypeHandler(),
                TypeCode.Int16 => new ShortTypeHandler(),
                TypeCode.Int32 => new IntTypeHandler(),
                TypeCode.Int64 => new LongTypeHandler(),
                TypeCode.Single => new SingleTypeHandler(),
                TypeCode.Double => new DoubleTypeHandler(),
                TypeCode.Decimal => new DecimalTypeHandler(),
                TypeCode.DateTime => new TimestampTypeHandler(DateStyle),
                TypeCode.Object => MapObjectType(typeof(T)),
                _ => throw new InvalidCastException()
            };

        protected virtual ITypeHandler MapObjectType(Type type)
        {
            if (type == typeof(DateTimeOffset))
                return new TimestampTzTypeHandler(DateStyle);

            if (type == typeof(DateOnly))
                return new DateTypeHandler(DateStyle);

            if (type == typeof(TimeOnly))
                return new TimeTypeHandler(DateStyle);

            if (type == typeof(TimeSpan))
                throw new NotImplementedException();

            throw new InvalidCastException();
        }
    }
}
