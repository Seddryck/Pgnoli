using Pgnoli.Messages.Backend.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Parse : CodeMessage
    {
        public const char Code = 'P';

        public ParsePayload Payload { get; private set; }

        internal Parse(byte[] bytes)
            : base(Code, bytes) { }

        internal Parse(ParsePayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Name.Length + 1 + Payload.Sql.Length + 1 + 2 + (Payload.ParameterCount * 4);
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Name);
            buffer.WriteAsciiStringNullTerminator(Payload.Sql);
            buffer.WriteShort(Payload.ParameterCount);
            foreach (var type in Payload.ParameterTypes)
                buffer.WriteInt(type);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var name = buffer.ReadStringUntilNullTerminator();
            var sql = buffer.ReadStringUntilNullTerminator();
            var count = buffer.ReadShort();

            Payload = new ParsePayload(name, sql, new List<int>(count));

            for (int i = 0; i < count; i++)
                Payload.ParameterTypes.Add(buffer.ReadInt());
        }

        public static ParseBuilder Sql(string value) => new(value);

        public record struct ParsePayload(string Name, string Sql, List<int> ParameterTypes)
        {
            public readonly short ParameterCount => (short)ParameterTypes.Count;
        }

        public class ParseBuilder : IMessageBuilder<Parse>
        {
            private ParsePayload Payload { get; set; }

            internal ParseBuilder(string sql)
            {
                Payload = new ParsePayload(string.Empty, sql, new());
            }

            public ParseBuilder Named(string name)
            {
                Payload = Payload with { Name = name };
                return this;
            }

            public ParseBuilder WithParameterType(int oid)
            {
                Payload.ParameterTypes.Add(oid);
                return this;
            }

            public Parse Build()
            {
                var msg = new Parse(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}