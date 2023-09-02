using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class BooleanTypeHandler : BaseTextTypeHandler<bool>
    {
        private const string TRUE = "t";
        private const string FALSE = "f";

        protected override string Convert(bool value)
                => value ? TRUE : FALSE;
        protected override bool Parse(string value)
            => value == TRUE || (value == FALSE ? false : throw new ArgumentOutOfRangeException());
    }
}
