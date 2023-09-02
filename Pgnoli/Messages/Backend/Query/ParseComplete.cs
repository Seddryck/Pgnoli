using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class ParseComplete : CodeMessage
    {
        public const char Code = '1';

        internal ParseComplete(byte[] bytes)
            : base(Code, bytes) { }

        internal ParseComplete()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 0;
        protected internal override void WritePayload(Buffer buffer) { }
        protected internal override void ReadPayload(Buffer buffer) { }

        public static ParseCompleteBuilder Message => new();

        public class ParseCompleteBuilder : IMessageBuilder<ParseComplete>
        {
            internal ParseCompleteBuilder()
            { }

            public ParseComplete Build()
            {
                var msg = new ParseComplete();
                msg.Write();
                return msg;
            }
        }
    }
}
