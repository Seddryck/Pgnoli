using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class SingleTypeHandler : BaseNumericTypeHandler<float>
    {
        public override byte[] GetBytes(float value)
            => GetBytes(Unsafe.As<float, int>(ref value));

        public override float Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<float>())
                throw new InvalidOperationException();

            var raw = ReadUnderlyingValue<int>(ref buffer);
            return Unsafe.As<int, float>(ref raw);
        }
    }
}
