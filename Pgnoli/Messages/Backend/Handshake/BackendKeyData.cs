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
    public class BackendKeyData : CodeMessage
    {
        public const char Code = 'K';
        public BackendKeyDataPayload Payload { get; private set; }

        public BackendKeyData(BackendKeyDataPayload payload)
            : base(Code) { Payload = payload; }

        public BackendKeyData(byte[] bytes)
            : base(Code, bytes) { }

        protected override int GetPayloadLength()
            => 4 + 4;
        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteInt(Payload.ProcessId);
            buffer.WriteInt(Payload.SecretKey);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            Payload = new BackendKeyDataPayload
            {
                ProcessId = buffer.ReadInt(),
                SecretKey = buffer.ReadInt(),
            };
        }

        public static BackendKeyDataBuilder Message(int processId, int secretKey) => new(processId, secretKey);

        public record struct BackendKeyDataPayload(int ProcessId, int SecretKey) { }

        public class BackendKeyDataBuilder : IMessageBuilder<BackendKeyData>
        {
            public BackendKeyDataPayload Payload { get; private set; }

            internal BackendKeyDataBuilder (int processId, int secretKey)
            {
                Payload = Payload with { ProcessId = processId, SecretKey = secretKey };
            }

            public BackendKeyData Build()
            {
                var msg = new BackendKeyData(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
