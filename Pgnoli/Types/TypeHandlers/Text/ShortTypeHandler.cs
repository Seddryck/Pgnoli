using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class ShortTypeHandler : BaseTextTypeHandler<short>
    {
        protected override string Convert(short value)
                => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override short Parse(string value)
            => short.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
