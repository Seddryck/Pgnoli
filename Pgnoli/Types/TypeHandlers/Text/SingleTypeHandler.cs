using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class SingleTypeHandler : BaseTextTypeHandler<float>
    {
        protected override string Convert(float value)
                => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override float Parse(string value)
            => float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
