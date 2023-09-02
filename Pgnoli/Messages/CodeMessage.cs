using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    public abstract class CodeMessage : Message
    {
        public char MessageType { get; }

        public CodeMessage(char messageType, byte[] bytes)
            : base(bytes) { (MessageType) = (messageType); }

        public CodeMessage(char messageType)
            : base() { (MessageType) = (messageType); }

        internal override byte[] Write()
        {
            Buffer.Allocate(1 + 4 + GetPayloadLength());
            Buffer.WriteAsciiChar(MessageType);
            Buffer.WriteInt(4 + GetPayloadLength());
            WritePayload(Buffer);
            return Buffer.GetBytes();
        }

        public override int Read()
        {
            if (Buffer is null)
                throw new ArgumentNullException(nameof(Buffer));

            Buffer.Reset();
            if (Buffer.ReadAsciiChar() != MessageType)
                throw new ArgumentOutOfRangeException(nameof(MessageType));

            Length = Buffer.ReadInt();
            if (Buffer.Length < Length + 1)
                throw new ArgumentOutOfRangeException(nameof(MessageType));
            else if (Buffer.Length > Length + 1)
                Buffer.TrimEnd(Length + 1);

            ReadPayload(Buffer);

            if (!Buffer.IsEnd())
                throw new ArgumentException(nameof(Buffer));
            return Length + 1;
        }
    }
}
