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
            => 1 + Payload.Name.Length + 1;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiChar(Payload.PortalType == PortalType.PreparedStatement ? 'S' : 'P');
            buffer.WriteAsciiStringNullTerminator(Payload.Name);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var portalType = buffer.ReadAsciiChar() == 'S' ? PortalType.PreparedStatement : PortalType.Portal;
            var name = buffer.ReadStringUntilNullTerminator();
            Payload = new DescribePayload(portalType, name);
        }


        public static DescribeBuilder UnnamedPreparedStatement => new(PortalType.PreparedStatement, string.Empty);
        public static DescribeBuilder UnnamedPortal => new(PortalType.Portal, string.Empty);
        public static DescribeBuilder PreparedStatement(string name) => new(PortalType.PreparedStatement, name);
        public static DescribeBuilder Portal(string name) => new(PortalType.Portal, name);

        public record struct DescribePayload(PortalType PortalType, string Name)
        { }

        public class DescribeBuilder : IMessageBuilder<Describe>
        {
            private DescribePayload Payload { get; set; }

            internal DescribeBuilder(PortalType value, string name)
            {
                Payload = new DescribePayload(value, name);
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