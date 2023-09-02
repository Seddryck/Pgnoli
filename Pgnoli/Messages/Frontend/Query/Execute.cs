using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static Pgnoli.Messages.Backend.Query.RowDescription;
using static Pgnoli.Messages.Frontend.Query.Describe;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Execute : CodeMessage
    {
        public const char Code = 'E';

        public ExecutePayload Payload { get; set; }

        internal Execute(byte[] bytes)
            : base(Code, bytes) { }

        internal Execute(ExecutePayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Name.Length + 1 + 4;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Name);
            buffer.WriteInt(Payload.MaxRowCount);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var name = buffer.ReadStringUntilNullTerminator();
            var maxRowCount = buffer.ReadInt();
            Payload = new ExecutePayload(name, maxRowCount);
        }

        public static ExecuteBuilder Portal(string name, int maxRowCount) => new(name, maxRowCount);
        public static ExecuteBuilder UnnamedPortal(int maxRowCount) => new(string.Empty, maxRowCount);

        public record struct ExecutePayload(string Name, int MaxRowCount)
        { }

        public class ExecuteBuilder : IMessageBuilder<Execute>
        {
            private ExecutePayload Payload { get; set; }

            public ExecuteBuilder (string name, int value)
            {
                Payload = new ExecutePayload(name, value);
            }

            public Execute Build()
            {
                var msg = new Execute(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}