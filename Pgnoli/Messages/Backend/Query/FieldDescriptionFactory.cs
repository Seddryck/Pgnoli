using Pgnoli.Types.PgTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Pgnoli.Messages.Backend.Query.FieldDescription;

namespace Pgnoli.Messages.Backend.Query
{
    internal class FieldDescriptionFactory
    {
        public FieldDescription Instantiate<T>(string name, EncodingFormat format)
            => Instantiate(typeof(T), name, format);

        public FieldDescription Instantiate(Type type, string name, EncodingFormat format)
            => Type.GetTypeCode(type) switch
            {
                TypeCode.String => Varchar(name, format),
                TypeCode.Char => BpChar(name, format),
                TypeCode.Boolean => Bool(name, format),
                TypeCode.Int16 => Smallint(name, format),
                TypeCode.Int32 => Int(name, format),
                TypeCode.Int64 => Bigint(name, format),
                TypeCode.Single => Real(name, format),
                TypeCode.Double => Float(name, format),
                TypeCode.Decimal => Numeric(name, format),
                TypeCode.DateTime => Timestamp(name, format),
                TypeCode.Object => MapObjectType(type, name, format),
                _ => throw new InvalidCastException()
            };

        protected virtual FieldDescription MapObjectType(Type type, string name, EncodingFormat format)
        {
            if (type == typeof(DateTimeOffset))
                return TimestampTz(name, format);

            if (type == typeof(DateOnly))
                return Date(name, format);

            if (type == typeof(TimeOnly))
                return Time(name, format);

            if (type == typeof(TimeSpan))
                return Interval(name, format);

            throw new InvalidCastException();
        }
    }
}