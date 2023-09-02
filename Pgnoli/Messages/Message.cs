using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    public abstract class Message
    {
        protected Buffer Buffer { get; }

        protected Encoding StringEncoding { get; } = Encoding.UTF8;

        public int Length { get; protected set; }

        public Message(byte[] bytes)
            => (Buffer) = (new Buffer(bytes));

        public Message()
            => (Buffer) = new Buffer();

        public byte[] GetBytes()
            => Buffer?.GetBytes() ?? throw new ArgumentNullException(nameof(Buffer));

        protected abstract int GetPayloadLength();
        internal virtual byte[] Write()
        {
            Buffer.Allocate(4 + GetPayloadLength());
            Buffer.WriteInt(4 + GetPayloadLength());
            WritePayload(Buffer);
            return Buffer.GetBytes();
        }
        protected internal abstract void WritePayload(Buffer buffer);

        public virtual int Read()
        {
            if (Buffer is null)
                throw new ArgumentNullException(nameof(Buffer));

            Buffer.Reset();

            Length = Buffer.ReadInt();
            if (Length < Buffer.Length)
                Buffer.TrimEnd(Length);

            ReadPayload(Buffer);

            if (!Buffer.IsEnd())
                throw new ArgumentException(nameof(Buffer));
            return Length;
        }
        protected internal abstract void ReadPayload(Buffer buffer);
    }
}
