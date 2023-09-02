using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Authentication
{
    public sealed class Password : CodeMessage
    {
        public const char Code = 'p';
        public PasswordPayload Payload { get; set; }

        internal Password(byte[] bytes)
            : base(Code, bytes) { }

        internal Password(PasswordPayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Value.Length;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Value);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            Payload = new PasswordPayload(buffer.ReadStringUntilNullTerminator());
        }

        public static PasswordBuilder Message(string password) => new(password);

        public record struct PasswordPayload(string Value) { }

        public class PasswordBuilder : IMessageBuilder<Password>
        {
            private PasswordPayload Payload { get; set; } = new();

            internal PasswordBuilder(string value)
            {
                Payload = Payload with { Value = value };
            }

            public Password Build()
            {
                var msg = new Password(new PasswordPayload());
                msg.Write();
                return msg;
            }
        }
    }
}
