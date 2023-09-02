using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class DateTypeHandler : BaseTimestampTypeHandler<DateOnly>
    {
        public DateTypeHandler(IDateStyle dateStyle)
            : base(dateStyle) { }

        protected override string Convert(DateOnly value)
                => value.ToString(DateStyle.DateFormat, FormatInfo);
        protected override DateOnly Parse(string value)
            => DateOnly.Parse(value, FormatInfo);
    }
}
