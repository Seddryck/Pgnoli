using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class TimestampTypeHandler : BaseTimestampTypeHandler<DateTime, long>
    {
        public TimestampTypeHandler()
            : base(new LongTypeHandler()) { }

        public override byte[] GetBytes(DateTime value)
            => GetUnderlyingBytes(value.Kind != DateTimeKind.Utc
                                    ? value.Subtract(Postgres_Epoch_Date).Ticks / 10
                                    : throw new ArgumentException());

        public override DateTime Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<long>())
                throw new InvalidOperationException();

            var value = ReadUnderlyingValue(ref buffer);
            return Postgres_Epoch_Date.AddTicks(value * 10);
        }
    }
}
