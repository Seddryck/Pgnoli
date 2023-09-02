using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal abstract class BaseNumericTypeHandler<T> : BaseBinaryTypeHandler<T>
    {
        public override byte[] GetBytes(T value)
            => GetBytes(value);

        protected byte[] GetBytes<Z>(Z value)
        {
            var bytes = (byte[])Array.CreateInstance(typeof(byte), Unsafe.SizeOf<Z>());
            Unsafe.WriteUnaligned(ref bytes[0], ApplyEndianness(value));
            return bytes;
        }

        public override T Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<T>())
                throw new InvalidOperationException();

            return ReadUnderlyingValue<T>(ref buffer);
        }

    }
}
