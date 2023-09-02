using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli
{
    public class Buffer
    {
        protected Encoding StringEncoding { get; } = Encoding.UTF8;
        private byte[]? Bytes { get; set; }
        public int Position { get; internal set; } = 0;

        public int Length => Bytes?.Length ?? throw new BufferNotAllocatedException();

        public Buffer(byte[] bytes)
            => Bytes = bytes;

        public Buffer()
        { }

        public void Allocate(int length)
        {
            if (Bytes is not null)
                throw new BufferAlreadyAllocatedException();
            Bytes = new byte[length];
        }

        public void TrimEnd(int length)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();
            Bytes = Bytes[0 .. length];
        }

        public void Reset()
            => Position = 0;

        public void Forward(int value)
            => Position += value;

        public bool IsEnd()
            => Position == Length;

        public byte[] GetBytes() => Bytes ?? throw new BufferNotAllocatedException();

        public byte Peek()
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (Position >= Length)
                throw new BufferOverflowException(Length, Position, 1);

            return Bytes[Position];
        }

        public short ReadShort()
            => ReadNumeric<short>();

        public int ReadInt()
            => ReadNumeric<int>();

        protected T ReadNumeric<T>()
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            var size = Unsafe.SizeOf<T>();
            if (size > Length - Position)
                throw new BufferOverflowException(Length, Position, size);

            var result = Unsafe.ReadUnaligned<T>(ref Bytes[Position]);
            result = ApplyEndianness(result);
            Position += size;
            return result;
        }

        protected T ApplyEndianness<T>(T value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;

            return value switch
            {
                short x => (T)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(T)),
                int x => (T)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(T)),
                long x => (T)Convert.ChangeType(BinaryPrimitives.ReverseEndianness(x), typeof(T)),
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            }; ;
        }

        public byte ReadByte()
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (Position >= Length)
                throw new BufferOverflowException(Length, Position, 1);

            return Bytes[Position++];
        }

        public char ReadAsciiChar()
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (Position > Length)
                throw new BufferOverflowException(Length, Position, 1);

            var c = Convert.ToChar(ReadByte());

            if (!char.IsAscii(c))
                throw new BufferUnexpectedCharException(c);
            return c;
        }

        public string ReadStringUntilNullTerminator()
        {
            var sb = new StringBuilder();
            var c = Convert.ToChar(ReadByte());
            while (c != '\0')
            {
                sb.Append(c);
                c = Convert.ToChar(ReadByte());
            }
            return sb.ToString();
        }

        public string ReadFixedSizeString(int size)
        {
            var sb = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                var c = Convert.ToChar(ReadByte());
                sb.Append(c);
            }
            return sb.ToString();
        }

        public byte[] ReadBytes(int size)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (size > Length - Position)
                throw new BufferOverflowException(Length, Position, size);

            var values = new byte[size];
            Bytes.AsSpan(Position..(Position + size)).CopyTo(values);
            Position += size;
            return values;
        }

        public void WriteByte(byte value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (Position >= Length)
                throw new BufferOverflowException(Length, Position, 1);

            Bytes[Position++] = value;
        }

        public void WriteAsciiChar(char value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (!char.IsAscii(value))
                throw new BufferUnexpectedCharException(value);

            WriteByte((byte)value);
        }

        public void WriteAsciiStringNullTerminator(string value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (value.Length > Length - Position + 1)
                throw new BufferOverflowException(Length, Position, value.Length);

            foreach (var c in value)
                WriteAsciiChar(c);
            WriteByte(0);
        }

        public void WriteFixedSizeString(string value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (value.Length > Length - Position + 1)
                throw new BufferOverflowException(Length, Position, value.Length);

            foreach (var c in value)
                WriteAsciiChar(c);
        }

        public void WriteString(string value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            var length = StringEncoding.GetByteCount(value);
            if (length > Length - Position + 1)
                throw new BufferOverflowException(Length, Position, length);

            StringEncoding.GetBytes(value).CopyTo(Bytes, Position);
            Position += length;
        }

        public void WriteShort(short value)
            => WriteNumeric(value);

        public void WriteInt(int value)
            => WriteNumeric(value);

        internal void WriteLong(long value)
            => WriteNumeric(value);

        protected void WriteNumeric<T>(T value)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            var size = Unsafe.SizeOf<T>();
            if (size > Length - Position + 1)
                throw new BufferOverflowException(Length, Position, size);

            Unsafe.WriteUnaligned(ref Bytes[Position], ApplyEndianness(value));
            Position += size;
        }

        public void WriteBytes(byte[] bytes)
        {
            if (Bytes is null)
                throw new BufferNotAllocatedException();

            if (bytes.Length > Length - Position + 1)
                throw new BufferOverflowException(Length, Position, bytes.Length);

            bytes.CopyTo(Bytes, Position);
            Position += bytes.Length;
        }
    }
}