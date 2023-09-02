using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class TimeTypeHandler : BaseTimestampTypeHandler<TimeOnly, long>
    {
        public TimeTypeHandler()
            : base(new LongTypeHandler()) { }

        public override byte[] GetBytes(TimeOnly value)
            => GetUnderlyingBytes(value.Ticks / 10);

        public override TimeOnly Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<long>())
                throw new InvalidOperationException();

            var value = ReadUnderlyingValue(ref buffer);
            return new(value * 10);
        }
    }
}
