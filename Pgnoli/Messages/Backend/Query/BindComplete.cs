using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class BindComplete : CodeMessage
    {
        public const char Code = '2';

        internal BindComplete(byte[] bytes)
            : base(Code, bytes) { }

        internal BindComplete()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 0;
        protected internal override void WritePayload(Buffer buffer) { }
        protected internal override void ReadPayload(Buffer buffer) { }

        public static BindCompleteBuilder Message => new();

        public class BindCompleteBuilder : IMessageBuilder<BindComplete>
        {
            public BindComplete Build()
            {
                var msg = new BindComplete();
                msg.Write();
                return msg;
            }
        }
    }
}
