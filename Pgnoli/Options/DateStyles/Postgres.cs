using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options
{
    internal class Postgres : IDateStyle
    {
        public string DateSeparator => " ";
        public string DateFormat => $"ddd/MMM/dd yyyy";
        public string TimestampFormat => $"ddd/MMM/dd HH:mm:ss yyyy";
        public string TimestampTzFormat => $"{DateFormat} {TimeFormat} UTC";
        public string TimeFormat => $"HH:mm:ss.ff";
    }
}
