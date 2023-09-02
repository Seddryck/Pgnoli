using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal class CharTypeHandler : BaseTextTypeHandler<char>
    {
        protected override string Convert(char value)
                => value.ToString();
        protected override char Parse(string value)
            => value.Length == 1 ? value[0] : throw new ArgumentOutOfRangeException();
    }
}
