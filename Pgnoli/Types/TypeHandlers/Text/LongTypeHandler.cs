using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class LongTypeHandler : BaseTextTypeHandler<long>
    {
        protected override string Convert(long value)
            => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override long Parse(string value)
            => long.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
