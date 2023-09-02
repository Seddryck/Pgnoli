using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class CloseComplete : CodeMessage
    {
        public const char Code = '3';

        internal CloseComplete(byte[] bytes)
            : base(Code, bytes) { }

        internal CloseComplete()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 0;
        protected internal override void WritePayload(Buffer buffer) { }
        protected internal override void ReadPayload(Buffer buffer) { }

        public static CloseCompleteBuilder Message => new();

        public class CloseCompleteBuilder : IMessageBuilder<CloseComplete>
        {
            internal CloseCompleteBuilder()
            { }

            public CloseComplete Build()
            {
                var msg = new CloseComplete();
                msg.Write();
                return msg;
            }
        }
    }
}
