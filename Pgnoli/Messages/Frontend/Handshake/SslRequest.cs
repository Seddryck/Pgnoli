using Pgnoli.Messages.Frontend.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Handshake
{
    public sealed class SslRequest : Message
    {
        public SslRequestPayload Payload { get; private set; }

        internal SslRequest(byte[] bytes)
            : base(bytes) { }

        internal SslRequest(SslRequestPayload payload)
            : base() { Payload = payload; }

        protected override int GetPayloadLength()
            => 4;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteShort(Payload.EndRequestCode);
            buffer.WriteShort(Payload.StartRequestCode);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var end = buffer.ReadShort();
            var start = buffer.ReadShort();
            Payload = new SslRequestPayload();
            if (start != Payload.StartRequestCode || end != Payload.EndRequestCode)
                throw new ArgumentException(nameof(buffer));
        }

        public static SslRequestBuilder Message => new();

        public record struct SslRequestPayload()
        {
            public short StartRequestCode { get; private set; } = 1234;
            public short EndRequestCode { get; private set; } = 5678;
        }

        public class SslRequestBuilder : IMessageBuilder<SslRequest>
        {
            private SslRequestPayload Payload { get; set; }

            internal SslRequestBuilder()
            {
                Payload = new SslRequestPayload();
            }

            public SslRequest Build()
            {
                var msg = new SslRequest(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
