using System;
using System.Reflection.Metadata;
using System.Text;


namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Query : CodeMessage
    {
        public const char Code = 'Q';

        public QueryPayload Payload { get; set; }

        internal Query(byte[] bytes)
            : base(Code, bytes) { }

        internal Query(QueryPayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Sql.Length + 1;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Sql);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var sql = buffer.ReadStringUntilNullTerminator();
            Payload = new QueryPayload(sql);
        }

        public static QueryBuilder Message(string sql) => new(sql);

        public record struct QueryPayload(string Sql)
        { }

        public class QueryBuilder : IMessageBuilder<Query>
        {
            private QueryPayload Payload { get; set; }

            public QueryBuilder (string sql)
            {
                Payload = new QueryPayload(sql);
            }

            public Query Build()
            {
                var msg = new Query(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}