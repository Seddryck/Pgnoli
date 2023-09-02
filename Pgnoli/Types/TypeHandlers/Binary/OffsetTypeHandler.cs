using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class OffsetTypeHandler : BaseTimestampTypeHandler<TimeSpan, int>
    {
        public OffsetTypeHandler()
            : base(new IntTypeHandler()) { }

        public override byte[] GetBytes(TimeSpan value)
            => GetUnderlyingBytes(-(int)(value.Ticks / TimeSpan.TicksPerSecond));

        public override TimeSpan Read(ref Buffer buffer)
            => new(ReadUnderlyingValue(ref buffer) * 10);
    }
}
