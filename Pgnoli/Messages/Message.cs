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
            => Buffer.GetBytes();

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
                throw new BufferNotAllocatedException();

            if (Buffer.Length == 0)
                throw new BufferEmptyException();

            Buffer.Reset();

            ReadPrefix(Buffer);

            Length = Buffer.ReadInt();
            if (Buffer.Length < Length + PrefixLength)
                throw new MessageUnexpectedLengthException(this.GetType(), Buffer.Length, Length);
            else if (Buffer.Length > Length + PrefixLength)
                Buffer.TrimEnd(Length + PrefixLength);

            ReadPayload(Buffer);

            if (!Buffer.IsEnd())
                throw new MessageNotFullyConsumedException(this.GetType(), Buffer.Length - Buffer.Position);
            return Length + PrefixLength;
        }
        protected internal abstract void ReadPayload(Buffer buffer);
        protected internal virtual void ReadPrefix(Buffer buffer) { }
        protected internal virtual int PrefixLength => 0;
    }
}
