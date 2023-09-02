using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class TimestampTzTypeHandler : BaseTimestampTypeHandler<DateTimeOffset>
    {
        public TimestampTzTypeHandler(IDateStyle dateStyle)
            : base(dateStyle) { }

        protected override string Convert(DateTimeOffset value)
                => value.ToString(DateStyle.TimestampTzFormat, FormatInfo);
        protected override DateTimeOffset Parse(string value)
            => DateTimeOffset.Parse(value, FormatInfo);
    }
}
