using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class TimeTypeHandler : BaseTimestampTypeHandler<TimeOnly>
    {
        public TimeTypeHandler(IDateStyle dateStyle)
            : base(dateStyle) { }

        protected override string Convert(TimeOnly value)
            => value.Millisecond == 0
                ? value.ToString(DateStyle.TimeFormat[..8], FormatInfo)
                : value.ToString(DateStyle.TimeFormat, FormatInfo).TrimEnd('0');

        protected override TimeOnly Parse(string value)
            => TimeOnly.Parse(value, FormatInfo);
    }
}
