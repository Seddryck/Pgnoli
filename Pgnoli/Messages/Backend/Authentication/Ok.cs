using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pgnoli.Messages.Backend.Query.RowDescription;

namespace Pgnoli.Messages.Backend.Authentication
{
    public sealed class Ok : CodeMessage
    {
        public const char Code = 'R';

        public Ok(byte[] bytes)
            : base(Code, bytes) { }

        public Ok()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 4;
        protected internal override void WritePayload(Buffer buffer)
            => buffer.WriteInt(0);
        protected internal override void ReadPayload(Buffer buffer)
        {
            if (buffer.ReadInt() != 0)
                throw new ArgumentException(nameof(buffer));
        }

        public static OkBuilder Message => new ();

        public class OkBuilder : IMessageBuilder<Ok>
        {
            internal OkBuilder()
            { }

            public Ok Build()
            {
                var msg = new Ok();
                msg.Write();
                return msg;
            }
        }
    }
}