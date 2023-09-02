using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Types.TypeHandlers;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal abstract class BaseBinaryTypeHandler<T> : BaseTypeHandler<T>
    {
        public abstract byte[] GetBytes(T value);

        public override void Write(T value, ref Buffer buffer)
        {
            var bytes = GetBytes(value);
            buffer.WriteInt(bytes.Length);
            buffer.WriteBytes(bytes);
        }

        protected Z ReadUnderlyingValue<Z>(ref Buffer buffer)
        {
            var size = Unsafe.SizeOf<Z>();
            if (size > buffer.Length - buffer.Position)
                throw new InvalidOperationException();

            var result = Unsafe.ReadUnaligned<Z>(ref buffer.GetBytes()[buffer.Position]);
            buffer.Forward(size);
            result = BaseBinaryTypeHandler<T>.ApplyEndianness(result);
            return result;
        }

        protected static Z ApplyEndianness<Z>(Z value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;

            return value switch
            {
                byte x => (Z)Convert.ChangeType(x, typeof(Z)),
                short x => (Z)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(Z)),
                int x => (Z)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(Z)),
                long x => (Z)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(Z)),
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }
    }
}
