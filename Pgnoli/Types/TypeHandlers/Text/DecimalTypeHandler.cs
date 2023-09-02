using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class DecimalTypeHandler : BaseTextTypeHandler<decimal>
    {
        protected override string Convert(decimal value)
                => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override decimal Parse(string value)
            => decimal.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
