using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class IntervalTypeHandler : BaseBinaryTypeHandler<TimeSpan>
    {
        protected LongTypeHandler MillisecondsHandler { get; set; }
        protected IntTypeHandler DaysHandler { get; set; }
        protected IntTypeHandler MonthsHandler { get; set; }

        public IntervalTypeHandler()
            => (MillisecondsHandler, DaysHandler, MonthsHandler) = (new(), new(), new());

        public override byte[] GetBytes(TimeSpan value)
        {
            var bytes = new byte[16];
            MillisecondsHandler.GetBytes((value.Ticks - TimeSpan.TicksPerDay * value.Days) / 10).CopyTo(bytes, 0);
            DaysHandler.GetBytes(value.Days).CopyTo(bytes, 8);
            MonthsHandler.GetBytes(0);
            return bytes;
        }

        public override TimeSpan Read(ref Buffer buffer)
            => new TimeSpan(MillisecondsHandler.Read(ref buffer) * 10)
                    .Add(new TimeSpan(DaysHandler.Read(ref buffer), 0, 0, 0))
                    .Add(MonthsHandler.Read(ref buffer) == 0 ? TimeSpan.Zero : throw new InvalidCastException());
    }
}
