using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class TimestampTypeHandler : BaseTimestampTypeHandler<DateTime>
    {
        public TimestampTypeHandler(IDateStyle dateStyle)
            : base(dateStyle) { }

        protected override string Convert(DateTime value)
            => value.Millisecond == 0
                ? value.ToString(DateStyle.TimestampFormat[0..^7], FormatInfo)
                : value.ToString(DateStyle.TimestampFormat, FormatInfo).TrimEnd('0');
        protected override DateTime Parse(string value)
            => DateTime.Parse(value, FormatInfo);
    }
}
