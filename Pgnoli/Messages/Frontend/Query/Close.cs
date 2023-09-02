using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;
using static Pgnoli.Messages.Backend.Query.RowDescription;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Close : CodeMessage
    {
        public const char Code = 'C';

        public ClosePayload Payload { get; private set; }

        internal Close(byte[] bytes)
            : base(Code, bytes) { }

        internal Close(ClosePayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Name.Length + 4;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiChar(Payload.PortalType == PortalType.PreparedStatement ? 'S' : 'P');
            buffer.WriteAsciiStringNullTerminator(Payload.Name);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var portalType = buffer.ReadAsciiChar() == 'S' ? PortalType.PreparedStatement : PortalType.Portal;
            var name = buffer.ReadFixedSizeString(buffer.Length - 6);
            Payload = new ClosePayload(portalType, name);
        }

        public static CloseBuilder PreparedStatement(string name) => new(PortalType.PreparedStatement, name);
        public static CloseBuilder Portal(string name) => new(PortalType.Portal, name);

        public record struct ClosePayload(PortalType PortalType, string Name)
        { }

        public class CloseBuilder : IMessageBuilder<Close>
        {
            private ClosePayload Payload { get; set; }

            internal CloseBuilder(PortalType value, string name)
            {
                Payload = Payload with { PortalType=value, Name = name };
            }

            public Close Build()
            {
                var msg = new Close(Payload);
                msg.Write();
                return msg;
            }
        }
    }

    public enum PortalType
    {
        PreparedStatement,
        Portal
    }
}
