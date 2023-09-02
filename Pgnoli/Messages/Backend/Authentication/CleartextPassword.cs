using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pgnoli.Messages.Backend.Query.RowDescription;

namespace Pgnoli.Messages.Backend.Authentication
{
    public sealed class CleartextPassword : CodeMessage
    {
        public const char Code = 'R';

        internal CleartextPassword(byte[] bytes)
            : base(Code, bytes) { }

        internal CleartextPassword()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 4;
        protected internal override void WritePayload(Buffer buffer)
            => buffer.WriteInt(3);
        protected internal override void ReadPayload(Buffer buffer)
        {
            if (buffer.ReadInt() != 3)
                throw new ArgumentException(nameof(buffer));
        }

        public static CleartextPasswordBuilder Message => new();

        public class CleartextPasswordBuilder : IMessageBuilder<CleartextPassword>
        {
            internal CleartextPasswordBuilder()
            { }

            public CleartextPassword Build()
            {
                var msg = new CleartextPassword();
                msg.Write();
                return msg;
            }
        }
    }
}