using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Handshake
{
    public sealed class ReadyForQuery : CodeMessage
    {
        private readonly char[] ValidStates = new[] { 'I', 'T', 'E' };
        public const char Code = 'Z';

        internal ReadyForQueryPayload Payload { get; private set; }

        internal ReadyForQuery(byte[] bytes)
            : base(Code, bytes) { }

        internal ReadyForQuery(ReadyForQueryPayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => 1;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiChar(Payload.State);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var state = buffer.ReadAsciiChar();
            if (!ValidStates.Contains(state))
                throw new ArgumentException(nameof(buffer));
            Payload = new ReadyForQueryPayload(state);
        }

        public static ReadyForQueryBuilder Idle => new ('I');
        public static ReadyForQueryBuilder TransactionBlock => new ('Z');
        public static ReadyForQueryBuilder FailedTransactionBlock => new ('E');

        internal record struct ReadyForQueryPayload(char State) { }

        public class ReadyForQueryBuilder : IMessageBuilder<ReadyForQuery>
        {
            private ReadyForQueryPayload Payload { get; set; }

            internal ReadyForQueryBuilder (char state)
            {
                Payload = Payload with { State = state };
            }

            public ReadyForQuery Build()
            {
                var msg = new ReadyForQuery(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
