using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class BooleanTypeHandler : BaseBinaryTypeHandler<bool>
    {
        public override byte[] GetBytes(bool value)
            => value ? new byte[] { 1 } : new byte[] { 0 };

        public override bool Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<byte>())
                throw new InvalidOperationException();

            return ReadUnderlyingValue<byte>(ref buffer) == Convert.ToByte((byte)1);
        }
    }
}
