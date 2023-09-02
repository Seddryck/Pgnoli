using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Types;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class RowDescription : CodeMessage
    {
        public const char Code = 'T';
        

        public RowDescriptionPayload Payload { get; private set; }

        internal RowDescription(RowDescriptionPayload payload)
            : base(Code) { Payload = payload; }

        internal RowDescription(byte[] bytes)
            : base(Code, bytes) { }

        protected override int GetPayloadLength()
            => 2 + Payload.Fields.Sum(x => x.Length);

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteShort((short)Payload.Fields.Count);
            foreach (var field in Payload.Fields)
            {
                buffer.WriteString(field.Name);
                buffer.WriteByte(byte.MinValue);
                buffer.WriteInt(field.TableObjectId);
                buffer.WriteShort(field.ColumnAttributeNumber);
                buffer.WriteInt(field.PgType.DataTypeObjectId);
                buffer.WriteShort(field.PgType.DataTypeSize);
                buffer.WriteInt(field.PgType.TypeModifier);
                buffer.WriteShort((short)field.FormatCode);
            }
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var fieldCount = buffer.ReadShort();
            Payload = new RowDescriptionPayload();
            var factory = new PgTypeFactory();
            for (int i = 0; i < fieldCount; i++)
            {
                var field = new FieldDescription()
                {
                    Name = buffer.ReadStringUntilNullTerminator(),
                    TableObjectId = buffer.ReadInt(),
                    ColumnAttributeNumber = buffer.ReadShort(),
                    PgType = factory.Instantiate(buffer.ReadInt(), buffer.ReadShort(), buffer.ReadInt()),
                    FormatCode = (EncodingFormat)buffer.ReadShort(),
                };
                Payload.Fields.Add(field);
            }
        }

        public record struct RowDescriptionPayload(List<FieldDescription> Fields)
        {
            public RowDescriptionPayload()
                : this(new List<FieldDescription>()) { }
        }

        public class RowDescriptionBuilder : IMessageBuilder<RowDescription>
        {
            public RowDescriptionPayload Payload { get; private set; } = new();
            private FieldDescriptionFactory Factory { get; set; } = new();

            internal RowDescriptionBuilder()
                => (Payload, Factory) = (new(), new());

            public RowDescriptionBuilder With(FieldDescription field)
            {
                Payload.Fields.Add(field);
                return this;
            }

            public RowDescriptionBuilder With<T>(string name, EncodingFormat format)
                => With(typeof(T), name, format);

            public RowDescriptionBuilder With(Type type, string name, EncodingFormat format)
                => With(Factory.Instantiate(type, name, format));

            public RowDescription Build()
            {
                var msg = new RowDescription(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
