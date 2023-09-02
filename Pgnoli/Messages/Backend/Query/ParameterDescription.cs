using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class ParameterDescription : CodeMessage
    {
        public const char Code = 't';

        private ParameterDescriptionPayload Payload { get; set; }

        internal ParameterDescription(ParameterDescriptionPayload payload)
            : base(Code) { Payload = payload; }

        internal ParameterDescription(byte[] bytes)
            : base(Code, bytes) { }

        protected override int GetPayloadLength()
            => 2 + Payload.ParameterTypes.Count * 4;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteShort((short)Payload.ParameterTypes.Count);
            foreach (var parameterType in Payload.ParameterTypes)
                buffer.WriteInt(parameterType);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var parameterCount = buffer.ReadShort();
            Payload = new ParameterDescriptionPayload();
            for (int i = 0; i < parameterCount; i++)
                Payload.AddParameterType(buffer.ReadInt());
        }

        internal record struct ParameterDescriptionPayload(List<int> ParameterTypes)
        {
            public ParameterDescriptionPayload()
                : this(new List<int>()) { }

            public void AddParameterType(int value)
                => ParameterTypes.Add(value);
        }

        public class ParameterDescriptionBuilder : IMessageBuilder<ParameterDescription>
        {
            private ParameterDescriptionPayload Payload { get; set; } = new();

            public ParameterDescriptionBuilder With(int parameterType)
            {
                Payload.ParameterTypes.Add(parameterType);
                return this;
            }

            public ParameterDescription Build()
            {
                var msg = new ParameterDescription(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
