using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Describe : CodeMessage
    {
        public const char Code = 'D';

        public DescribePayload Payload { get; private set; }

        internal Describe(byte[] bytes)
            : base(Code, bytes) { }

        internal Describe(DescribePayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Math.Max(Payload.Name?.Length ?? 0, 1) + 1;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiChar(Payload.PortalType == PortalType.PreparedStatement ? 'S' : 'P');
            buffer.WriteFixedSizeString(Payload.Name);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var portalType = buffer.ReadAsciiChar() == 'S' ? PortalType.PreparedStatement : PortalType.Portal;
            var name = buffer.ReadFixedSizeString(buffer.Length - 6);
            Payload = new DescribePayload(portalType, name);
        }


        public static DescribeBuilder PreparedStatement => new(PortalType.PreparedStatement);
        public static DescribeBuilder Portal => new(PortalType.Portal);

        public record struct DescribePayload(PortalType PortalType, string Name)
        { }

        public class DescribeBuilder : IMessageBuilder<Describe>
        {
            private DescribePayload Payload { get; set; }

            public DescribeBuilder(PortalType value)
            {
                Payload = new DescribePayload(value, string.Empty);
            }

            public DescribeBuilder Named(string name)
            {
                Payload = Payload with { Name = name };
                return this;
            }

            public Describe Build()
            {
                var msg = new Describe(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}