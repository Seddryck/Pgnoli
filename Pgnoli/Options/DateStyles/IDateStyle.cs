using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options.DateStyles
{
    public interface IDateStyle
    {
        string DateSeparator { get; }
        string TimestampFormat { get; }
        string TimestampTzFormat { get; }
        string TimeFormat { get; }
        string DateFormat { get; }
    }
}
