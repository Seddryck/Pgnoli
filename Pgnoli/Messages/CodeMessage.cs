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

        protected internal override void ReadPrefix(Buffer buffer)
        {
            var code = buffer.ReadAsciiChar();
            if (code != MessageType)
                throw new MessageMismatchCodeException(this.GetType(), MessageType, code);
        }

        protected internal override int PrefixLength => 1;
    }
}
