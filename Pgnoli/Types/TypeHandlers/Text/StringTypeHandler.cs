using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class StringTypeHandler : BaseTextTypeHandler<string>
    {
        protected override string Convert(string value)
                => value;
        protected override string Parse(string value)
            => value;
    }
}
