using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Sync : CodeMessage
    {
        public const char Code = 'S';

        internal Sync(byte[] bytes)
            : base(Code, bytes) { }

        internal Sync()
            : base(Code) { }

        protected override int GetPayloadLength()
            => 0;
        protected internal override void WritePayload(Buffer buffer) { }
        protected internal override void ReadPayload(Buffer buffer) { }

        public static SyncBuilder Message => new();

        public class SyncBuilder : IMessageBuilder<Sync>
        {
            internal SyncBuilder()
            { }

            public Sync Build()
            {
                var msg = new Sync();
                msg.Write();
                return msg;
            }
        }
    }
}
