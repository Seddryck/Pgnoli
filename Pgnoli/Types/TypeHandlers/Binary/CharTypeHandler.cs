using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class CharTypeHandler : BaseBinaryTypeHandler<char>
    {
        public override byte[] GetBytes(char value)
            => Encoding.UTF8.GetBytes(value.ToString());

        public override char Read(ref Buffer buffer)
            => Convert.ToChar(buffer.ReadByte());
    }
}
