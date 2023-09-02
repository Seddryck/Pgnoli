using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class UnsignedIntTypeHandler : BaseTextTypeHandler<uint>
    {
        protected override string Convert(uint value)
                => value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        protected override uint Parse(string value)
            => uint.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}
