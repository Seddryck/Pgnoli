using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Authentication
{
    public sealed class MD5Password : CodeMessage
    {
        public const char Code = 'R';

        public MD5PasswordPayload Payload { get; set; }

        internal MD5Password (byte[] bytes)
            : base(Code, bytes) { }

        internal MD5Password (MD5PasswordPayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => 4;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteInt(5);
            buffer.WriteBytes(Payload.Salt);
        }
            
        protected internal override void ReadPayload(Buffer buffer)
        {
            if (buffer.ReadInt() != 5)
                throw new ArgumentException(nameof(buffer));
            Payload = new MD5PasswordPayload(buffer.ReadBytes(4));
        }

        public static MD5PasswordBuilder Message(byte[] salt) => new(salt);

        public record struct MD5PasswordPayload(byte[] Salt) { }

        public class MD5PasswordBuilder : IMessageBuilder<MD5Password >
        {
            private MD5PasswordPayload Payload { get; set; }

            internal MD5PasswordBuilder (byte[] salt)
            {
                Payload = Payload with { Salt = salt };
            }

            public MD5Password Build()
            {
                var msg = new MD5Password (new MD5PasswordPayload());
                msg.Write();
                return msg;
            }
        }
    }
}