using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pgnoli.Options.DateStyles
{
    internal abstract class Iso : IDateStyle
    {
        public string DateSeparator => "-";
        public abstract string DateFormat { get; }
        public string TimestampFormat => $"{DateFormat} {TimeFormat}";
        public string TimestampTzFormat => $"{DateFormat} {TimeFormat}zz";
        public string TimeFormat => $"HH:mm:ss.ffffff";
    }
}
