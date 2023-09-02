using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend.Query
{
    public sealed class DataRow : CodeMessage
    {
        public const char Code = 'D';
        public DataRowPayload Payload { get; private set; }

        internal DataRow(DataRowPayload payload)
            : base(Code) { (Payload) = (payload); }

        internal DataRow(byte[] bytes, FieldDescription[] fieldDescriptions, TypeHandlerFactory typeHandlerFactory)
            : base(Code, bytes) { Payload = new DataRowPayload(fieldDescriptions, typeHandlerFactory); }

        protected override int GetPayloadLength()
            => 8 * 256 - 4 - 1;

        internal override byte[] Write()
        {
            base.Write();
            Buffer.TrimEnd(Buffer.Position);
            Buffer.Position = 1;
            Buffer.WriteInt(Buffer.Length - 1);
            return Buffer.GetBytes();
        }

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteShort((short)Payload.Values.Count);
            var init = Buffer.Position;
            for (int i = 0; i < Payload.Values.Count; i++)
            {
                var writer = Payload.TypeHandlerFactory.Instantiate(Payload.FieldDescriptions[i]);
                writer.Write(Payload.Values[i], ref buffer);
            }
            Length = Buffer.Position - init;
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var fieldCount = buffer.ReadShort();
            for (int i = 0; i < fieldCount; i++)
            {
                var reader = Payload.TypeHandlerFactory.Instantiate(Payload.FieldDescriptions[i]);
                var value = reader.Read(ref buffer);
                Payload.Values.Add(value);
            }
        }

        public record struct DataRowPayload(FieldDescription[] FieldDescriptions, TypeHandlerFactory TypeHandlerFactory, List<object> Values)
        {
            public DataRowPayload(FieldDescription[] fieldDescriptions, TypeHandlerFactory typeHandlerFactory)
                : this(fieldDescriptions, typeHandlerFactory, new List<object>()) { }
        }

        public class DataRowBuilder : IMessageBuilder<DataRow>
        {
            public DataRowPayload Payload { get; private set; }

            public DataRowBuilder(FieldDescription[] fieldDescriptions, TypeHandlerFactory typeHandlerFactory)
                => Payload = new DataRowPayload(fieldDescriptions, typeHandlerFactory);

            public DataRowBuilder With(object field)
            {
                Payload.Values.Add(field);
                return this;
            }

            public DataRow Build()
            {
                var msg = new DataRow(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
