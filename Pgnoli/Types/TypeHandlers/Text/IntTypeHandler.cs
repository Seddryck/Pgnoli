using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class IntTypeHandler : BaseTextTypeHandler<int>
    {
        protected override string Convert(int value)
            => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override int Parse(string value)
            => int.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
