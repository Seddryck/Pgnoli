using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class UnsignedLongTypeHandler : BaseTextTypeHandler<ulong>
    {
        protected override string Convert(ulong value)
            => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override ulong Parse(string value)
            => ulong.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
