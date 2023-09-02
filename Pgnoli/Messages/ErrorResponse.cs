using System.Text;
using static Pgnoli.Messages.Backend.Query.RowDescription;

namespace Pgnoli.Messages
{
    internal class ErrorResponse : CodeMessage
    {
        protected ErrorResponseBuilder? Payload { get; }

        private Dictionary<char, string>? items;
        public Dictionary<char, string> Items
        {
            get 
            { 
                items ??= Initialize();
                return items;
            }
        }

        public ErrorResponse(ErrorResponseBuilder payload)
            : base('T') { Payload = payload; }

        public ErrorResponse(byte[] bytes)
            : base('T', bytes) { }

        protected override int GetPayloadLength()
            => Items.Values.Sum(x => x.Length);

        protected virtual Dictionary<char, string> Initialize()
        {
            if (Payload is null)
                throw new ArgumentNullException(nameof(Payload));

            var label = Enum.GetName(typeof(ErrorSeverity), Payload.Severity)
                                ?? throw new ArgumentOutOfRangeException(nameof(Payload.Severity));

            return new Dictionary<char, string>()
            {
                { 'S', label },
                { 'V', label },
                { 'M', Payload.Message ?? throw new ArgumentNullException(nameof(Payload.Message)) },
            };
        }

        protected internal override void WritePayload(Buffer buffer)
        {
            foreach (var item in Items)
            {
                buffer.WriteAsciiChar(item.Key);
                buffer.WriteString(item.Value);
                buffer.WriteByte(0);
            }
            buffer.WriteByte(0);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            Items.Clear();

            var key = buffer.ReadAsciiChar();
            while(key!='\0')
            {
                var value = buffer.ReadStringUntilNullTerminator();
                Items.Add(key, value);
                key = buffer.ReadAsciiChar();
            }
        }

        internal class ErrorResponseBuilder : IMessageBuilder<ErrorResponse>
        {
            public ErrorSeverity Severity { get; private set; } = ErrorSeverity.Error;
            public string? Message { get; private set; }

            public ErrorResponseBuilder With(ErrorSeverity severity)
            {
                Severity = severity;
                return this;
            }

            public ErrorResponseBuilder WithMessage(string message)
            {
                Message = message;
                return this;
            }

            public ErrorResponse Build()
            {
                var msg = new ErrorResponse(this);
                msg.Write();
                return msg;
            }
        }
    }
}
