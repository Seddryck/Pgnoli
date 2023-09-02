using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class DoubleTypeHandler : BaseTextTypeHandler<double>
    {
        protected override string Convert(double value)
                    => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override double Parse(string value)
            => double.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
