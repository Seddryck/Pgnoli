using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Types.TypeHandlers.Binary;
using Pgnoli.Types.TypeHandlers;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class BinaryTypeHandlerFactory : ITypeHandlerFactory
    {
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
                TypeCode.DateTime => new TimestampTypeHandler(),
                TypeCode.Object => MapObjectType(typeof(T)),
                _ => throw new InvalidCastException()
            };

        protected virtual ITypeHandler MapObjectType(Type type)
        {
            if (type == typeof(DateTimeOffset))
                return new TimestampTzTypeHandler();

            if (type == typeof(DateOnly))
                return new DateTypeHandler();

            if (type == typeof(TimeOnly))
                return new TimeTypeHandler();

            if (type == typeof(TimeSpan))
                return new IntervalTypeHandler();

            throw new InvalidCastException();
        }
    }
}
