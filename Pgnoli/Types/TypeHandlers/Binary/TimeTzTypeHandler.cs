using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class TimeTzTypeHandler : BaseBinaryTypeHandler<DateTimeOffset>
    {
        protected TimeTypeHandler TimeTypeHandler { get; set; }
        protected OffsetTypeHandler OffsetTypeHandler { get; set; }

        public TimeTzTypeHandler()
            => (TimeTypeHandler, OffsetTypeHandler) = (new(), new());

        public override byte[] GetBytes(DateTimeOffset value)
        {
            var bytes = new byte[12];
            TimeTypeHandler.GetBytes(TimeOnly.FromTimeSpan(value.TimeOfDay)).CopyTo(bytes, 0);
            OffsetTypeHandler.GetBytes(value.Offset).CopyTo(bytes, 8);
            return bytes;
        }

        public override DateTimeOffset Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<long>() + Unsafe.SizeOf<int>())
                throw new InvalidOperationException();
            var ticks = TimeTypeHandler.ReadUnderlyingValue(ref buffer) * 10;
            var offset = OffsetTypeHandler.ReadUnderlyingValue(ref buffer);
            return new(new DateTime(2000, 1, 1).AddTicks(ticks), TimeSpan.FromSeconds(-offset));
        }
    }
}