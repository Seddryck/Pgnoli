using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class TimestampTzTypeHandler : BaseBinaryTypeHandler<DateTimeOffset>
    {
        private readonly TimestampTypeHandler UnderlyingTypeHandler = new();

        public override byte[] GetBytes(DateTimeOffset value)
            => UnderlyingTypeHandler.GetBytes(value.ToUniversalTime().DateTime);

        public override DateTimeOffset Read(ref Buffer buffer)
        {
            var value = UnderlyingTypeHandler.Read(ref buffer);
            return new(value, TimeSpan.Zero);
        }
    }
}
