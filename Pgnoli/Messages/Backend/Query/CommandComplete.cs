using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class CommandComplete : CodeMessage
    {
        public const char Code = 'C';

        public CommandCompletePayload Payload { get; private set; }

        internal CommandComplete(byte[] bytes)
            : base(Code, bytes) { }

        internal CommandComplete(CommandCompletePayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Label.Length + 1;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Label);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var label = buffer.ReadStringUntilNullTerminator();
            var tokens = label.Split(' ');
            if (tokens.Length != 2)
                throw new ArgumentException(nameof(buffer));
            Payload = int.TryParse(tokens[1], out var rowCount)
                        ? new CommandCompletePayload(tokens[0], rowCount)
                        : throw new ArgumentException(nameof(buffer));
        }

        public static CommandCompleteBuilder Insert => new ("INSERT", 0);
        public static CommandCompleteBuilder Delete(int rowCount) => new("DELETE", rowCount);
        public static CommandCompleteBuilder Update(int rowCount) => new("UPDATE", rowCount);
        public static CommandCompleteBuilder Merge(int rowCount) => new("MERGE", rowCount);
        public static CommandCompleteBuilder Select(int rowCount) => new ("SELECT", rowCount);
        public static CommandCompleteBuilder Move(int rowCount) => new("MOVE", rowCount);
        public static CommandCompleteBuilder Fetch(int rowCount) => new("FETCH", rowCount);
        public static CommandCompleteBuilder Copy(int rowCount) => new("COPY", rowCount);

        public record struct CommandCompletePayload(string Tag, int RowCount)
        {
            public readonly string Label => $"{Tag} {RowCount}";
        }

        public class CommandCompleteBuilder : IMessageBuilder<CommandComplete>
        {
            public CommandCompletePayload Payload { get; private set; }

            internal CommandCompleteBuilder (string tag, int rowCount)
            {
                Payload = Payload with { Tag = tag, RowCount = rowCount };
            }

            public CommandComplete Build()
            {
                var msg = new CommandComplete(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
