using Pgnoli.Options.DateStyles;
using Pgnoli.Messages.Backend.Query;
using Pgnoli.Types.TypeHandlers.Binary;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pgnoli.Messages.Backend.Query.FieldDescription;
using Pgnoli.Messages;

namespace Pgnoli.Types.TypeHandlers
{
    public class TypeHandlerFactory
    {
        protected Dictionary<EncodingFormat, ITypeHandlerFactory> Factories { get; }

        public TypeHandlerFactory(IDateStyle dateStyle)
        {
            Factories = new()
            {
                { EncodingFormat.Binary, new BinaryTypeHandlerFactory() },
                { EncodingFormat.Text, new TextTypeHandlerFactory(dateStyle) }
            };
        }

        public ITypeHandler Instantiate(FieldDescription desc)
        {
            if (Factories.TryGetValue(desc.FormatCode, out var factory))
                return desc.PgType.DataTypeObjectId switch
                {
                    1043 => factory.Instantiate<string>(),
                    18 => factory.Instantiate<char>(),
                    16 => factory.Instantiate<bool>(),
                    21 => factory.Instantiate<short>(),
                    23 => factory.Instantiate<int>(),
                    20 => factory.Instantiate<long>(),
                    700 => factory.Instantiate<float>(),
                    701 => factory.Instantiate<double>(),
                    1700 => factory.Instantiate<decimal>(),
                    1082 => factory.Instantiate<DateOnly>(),
                    1083 => factory.Instantiate<TimeOnly>(),
                    1114 => factory.Instantiate<DateTime>(),
                    1266 => factory.Instantiate<DateTimeOffset>(),
                    1186 => factory.Instantiate<TimeSpan>(),
                    _ => throw new NotImplementedException()
                };
            else
                throw new ArgumentOutOfRangeException();
        }
    }
}
