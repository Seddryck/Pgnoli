using System.Text;
using System.Xml.Linq;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class ErrorResponse : CodeMessage
    {
        public const char Code = 'E';

        public ErrorResponsePayload Payload { get; private set; }

        internal ErrorResponse(ErrorResponsePayload payload)
            : base(Code) { Payload = payload; }

        internal ErrorResponse(byte[] bytes)
            : base(Code, bytes) { }

        protected override int GetPayloadLength()
            => 1 + Enum.GetName(typeof(ErrorSeverity), Payload.Severity)!.Length + 1 +
                1 + Payload.SqlState.Length + 1 +
                1 + Payload.Message.Length + 1 +
                Payload.OptionalMessages.Sum(x => 1 + x.Value.Length + 1) +
                1;

        protected internal override void WritePayload(Buffer buffer)
        {
            foreach (var item in Payload.OptionalMessages
                .Prepend(new KeyValuePair<char, string>('M', Payload.Message))
                .Prepend(new KeyValuePair<char, string>('C', Payload.SqlState))
                .Prepend(new KeyValuePair<char, string>('S', Enum.GetName(typeof(ErrorSeverity), Payload.Severity)!)))
            {
                buffer.WriteAsciiChar(item.Key);
                buffer.WriteString(item.Value);
                buffer.WriteByte(0);
            }
            buffer.WriteByte(0);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            Payload = Payload with { OptionalMessages = new() };
            var key = buffer.ReadAsciiChar();
            while (key != '\0')
            {
                var value = buffer.ReadStringUntilNullTerminator();
                switch (key)
                {
                    case 'S': Payload = Payload with { Severity = (ErrorSeverity)Enum.Parse(typeof(ErrorSeverity), value) }; break;
                    case 'C': Payload = Payload with { SqlState = value }; break;
                    case 'M': Payload = Payload with { Message = value }; break;
                    default:
                        Payload.OptionalMessages.Add(key, value);
                        break;
                }
                key = buffer.ReadAsciiChar();
            }
        }

        public static ErrorResponseBuilder Error(string sqlState, string message)
            => new(ErrorSeverity.Error, sqlState, message);

        public static ErrorResponseBuilder Fatal(string sqlState, string message)
            => new(ErrorSeverity.Fatal, sqlState, message);

        public static ErrorResponseBuilder Panic(string sqlState, string message)
            => new(ErrorSeverity.Panic, sqlState, message);

        public static ErrorResponseBuilder Warning(string sqlState, string message)
            => new(ErrorSeverity.Warning, sqlState, message);

        public static ErrorResponseBuilder Notice(string sqlState, string message)
           => new(ErrorSeverity.Notice, sqlState, message);

        public static ErrorResponseBuilder Debug(string sqlState, string message)
           => new(ErrorSeverity.Debug, sqlState, message);

        public static ErrorResponseBuilder Info(string sqlState, string message)
           => new(ErrorSeverity.Info, sqlState, message);

        public static ErrorResponseBuilder Log(string sqlState, string message)
           => new(ErrorSeverity.Log, sqlState, message);

        public record struct ErrorResponsePayload(ErrorSeverity Severity, string SqlState, string Message, Dictionary<char, string> OptionalMessages)
        { }

        public class ErrorResponseBuilder : IMessageBuilder<ErrorResponse>
        {
            private ErrorResponsePayload Payload { get; set; }

            public ErrorResponseBuilder(ErrorSeverity severity, string sqlState, string message)
                => Payload = new ErrorResponsePayload(severity, sqlState, message, new Dictionary<char, string>());

            public ErrorResponseBuilder With(char key, string value)
            {
                Payload.OptionalMessages.Add(key, value);
                return this;
            }

            public ErrorResponse Build()
            {
                var msg = new ErrorResponse(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
