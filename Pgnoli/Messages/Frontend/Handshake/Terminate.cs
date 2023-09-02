using Pgnoli.Messages.Frontend.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Handshake
{
    public class Terminate : CodeMessage
    {
        public const char Code = 'X';

        internal Terminate(byte[] bytes)
            : base(Code, bytes) { }

        internal Terminate()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 0;

        protected internal override void WritePayload(Buffer buffer) { }
        protected internal override void ReadPayload(Buffer buffer) { }

        public static TerminateBuilder Message => new();
        
        public class TerminateBuilder : IMessageBuilder<Terminate>
        {
            internal TerminateBuilder()
            { }

            public Terminate Build()
            {
                var msg = new Terminate();
                msg.Write();
                return msg;
            }
        }
    }
}
