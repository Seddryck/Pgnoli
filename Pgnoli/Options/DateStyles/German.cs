using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options.DateStyles
{
    internal class German : IDateStyle
    {
        public string DateSeparator => ".";
        public string DateFormat => $"dd/MM/yyyy";
        public string TimestampFormat => $"{DateFormat} {TimeFormat}";
        public string TimestampTzFormat => $"{DateFormat} {TimeFormat} UTC";
        public string TimeFormat => $"HH:mm:ss";
    }
}
