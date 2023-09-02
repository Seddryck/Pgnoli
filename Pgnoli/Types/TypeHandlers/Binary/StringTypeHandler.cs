using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class StringTypeHandler : BaseBinaryTypeHandler<string>
    {
        public override byte[] GetBytes(string value)
            => Encoding.UTF8.GetBytes(value);

        public override string Read(ref Buffer buffer)
        {
            var length = buffer.ReadInt();
            var bytes = buffer.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
