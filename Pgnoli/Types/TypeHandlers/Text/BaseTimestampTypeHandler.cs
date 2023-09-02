using Pgnoli.Options.DateStyles;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal abstract class BaseTimestampTypeHandler<T> : BaseTextTypeHandler<T>
    {
        protected IDateStyle DateStyle { get; }
        protected DateTimeFormatInfo FormatInfo { get; set; }

        public BaseTimestampTypeHandler(IDateStyle dateStyle)
        {
            DateStyle = dateStyle;
            FormatInfo = CultureInfo.CreateSpecificCulture("en-us").DateTimeFormat;
            FormatInfo.DateSeparator = dateStyle.DateSeparator;
            FormatInfo.ShortDatePattern = dateStyle.DateFormat;
            FormatInfo.FullDateTimePattern = dateStyle.TimestampFormat;
            FormatInfo.ShortTimePattern = dateStyle.TimeFormat;
        }
    }
}
