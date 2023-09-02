using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class DoubleTypeHandler : BaseNumericTypeHandler<double>
    {
        public override byte[] GetBytes(double value)
            => GetBytes(Unsafe.As<double, long>(ref value));

        public override double Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<double>())
                throw new InvalidOperationException();

            var raw = ReadUnderlyingValue<long>(ref buffer);
            return Unsafe.As<long, double>(ref raw);
        }
    }
}
