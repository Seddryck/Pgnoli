using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pgnoli.Messages.Backend.Query.RowDescription;
using static Pgnoli.Messages.Backend.Authentication.Ok;

namespace Pgnoli.Messages.Backend.Handshake
{
    public class ParameterStatus : CodeMessage
    {
        public const char Code = 'S';
        public ParameterStatusPayload Payload { get; private set; }

        public ParameterStatus(ParameterStatusPayload payload)
            : base(Code) { Payload = payload; }

        public ParameterStatus(byte[] bytes)
            : base(Code, bytes) { }

        protected override int GetPayloadLength()
            => Payload.Key.Length + 1 + Payload.Value.Length + 1;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Key);
            buffer.WriteAsciiStringNullTerminator(Payload.Value);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            Payload = new ParameterStatusPayload
            {
                Key = buffer.ReadStringUntilNullTerminator(),
                Value = buffer.ReadStringUntilNullTerminator(),
            };
        }

        public static ParameterStatusBuilder ServerVersion(string version) => new("server_version", version);
        public static ParameterStatusBuilder ClientEncoding(string client_encoding) => new("client_encoding", client_encoding);
        public static ParameterStatusBuilder ServerEncoding(string client_encoding) => new("server_encoding", client_encoding);
        public static ParameterStatusBuilder WithIntegerDateTimes() => new("integer_datetimes", "on");
        public static ParameterStatusBuilder WithoutIntegerDateTimes() => new("integer_datetimes", "off");
        public static ParameterStatusBuilder Status(string key, string value) => new(key, value);

        public record struct ParameterStatusPayload(string Key, string Value) { }

        public class ParameterStatusBuilder : IMessageBuilder<ParameterStatus>
        {
            private ParameterStatusPayload Payload { get; set; }

            internal ParameterStatusBuilder(string key, string value)
            {
                Payload = new (key,value);
            }

           

            public ParameterStatus Build()
            {
                var msg = new ParameterStatus(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
