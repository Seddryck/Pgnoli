using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Pgnoli.Messages.Frontend.Query
{
    public sealed class Bind : CodeMessage
    {
        public const char Code = 'B';

        public BindPayload Payload { get; private set; }

        internal Bind(byte[] bytes)
            : base(Code, bytes) { }

        internal Bind(BindPayload payload)
            : base(Code) { Payload = payload; }

        protected override int GetPayloadLength()
            => Payload.Destination.Length + 1 + Payload.Source.Length + 1
                    + 2 * Math.Max(1, GetFormatValue(Payload.Parameters.Select(x => x.EncodingFormat).ToArray())) + 2
                    + 2 * Payload.Parameters.Count + Payload.Parameters.Select(x => x.Value?.Length ?? 0).Sum()
                    + 2 + 2 * Payload.ResultsFormat.Length
               ;

        private int GetFormatValue(EncodingFormat[] formats)
            => Payload.Parameters.All(x => x.EncodingFormat == EncodingFormat.Text)
                ? 0
                : Payload.Parameters.All(x => x.EncodingFormat == EncodingFormat.Binary)
                    ? 1
                    : Payload.Parameters.Count;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteAsciiStringNullTerminator(Payload.Destination);
            buffer.WriteAsciiStringNullTerminator(Payload.Source);
            if (Payload.Parameters.All(x => x.EncodingFormat == EncodingFormat.Text))
                buffer.WriteShort(0);
            else if (Payload.Parameters.All(x => x.EncodingFormat == EncodingFormat.Binary))
                buffer.WriteShort(1);
            else
            {
                buffer.WriteShort((short)Payload.Parameters.Count);
                foreach (var parameter in Payload.Parameters)
                    buffer.WriteShort((short)parameter.EncodingFormat);
            }
            buffer.WriteShort((short)Payload.Parameters.Count);

            foreach (var parameter in Payload.Parameters)
            {
                buffer.WriteShort((short)(parameter.Value?.Length ?? -1));
                if (parameter.Value is not null)
                    buffer.WriteBytes(parameter.Value);
            }

            buffer.WriteShort((short)Payload.ResultsFormat.Length);
            foreach (var format in Payload.ResultsFormat.GetAll())
                buffer.WriteShort((short)format);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var destination = buffer.ReadStringUntilNullTerminator();
            var source = buffer.ReadStringUntilNullTerminator();
            Payload = new BindPayload(destination, source, new List<ParameterInfo>(), new AllResultsAsText());

            var countFormat = buffer.ReadShort();
            var formats = new List<EncodingFormat>();
            if (countFormat > 1)
                for (int i = 0; i < countFormat; i++)
                    formats.Add((EncodingFormat)buffer.ReadShort());

            var count = buffer.ReadShort();
            for (int i = 0; i < count; i++)
            {
                var length = buffer.ReadShort();
                Payload.AddParameter(countFormat <= 1 ? (EncodingFormat)countFormat : formats[i]
                                        , length == -1 ? null : buffer.ReadBytes(length));
            }

            var countResults = buffer.ReadShort();
            if (countResults == 1)
            {
                Payload = Payload with { ResultsFormat = (EncodingFormat)buffer.ReadShort() == EncodingFormat.Binary ? new AllResultsAsBinary() : new AllResultsAsText() };
            }
            else
            {
                var resultsFormat = new ResultsFormat();
                for (int i = 0; i < countResults; i++)
                    resultsFormat.AddFormat((EncodingFormat)buffer.ReadShort());
                Payload = Payload with { ResultsFormat = resultsFormat };
            }
        }

        public static BindBuilder Portal(string destination, string source) => new (destination, source);
        public static BindBuilder UnnamedPortal => new();

        public record struct BindPayload(string Destination, string Source, List<ParameterInfo> Parameters, IResultsFormat ResultsFormat)
        {
            public void AddParameter(EncodingFormat format, byte[]? value)
                => Parameters.Add(new ParameterInfo(format, value));
        }

        public interface IResultsFormat
        {
            EncodingFormat[] GetAll();
            int Length { get; }
        }

        internal class AllResultsAsText : IResultsFormat
        {
            public EncodingFormat[] GetAll() => new EncodingFormat[] { EncodingFormat.Text };
            public int Length => 1;
        }

        internal class AllResultsAsBinary : IResultsFormat
        {
            public EncodingFormat[] GetAll() => new EncodingFormat[] { EncodingFormat.Binary };
            public int Length => 1;
        }

        internal class ResultsFormat : IResultsFormat
        {
            private readonly List<EncodingFormat> formats = new();
            public EncodingFormat[] GetAll() => formats.ToArray();
            public int Length => formats.Count;
            public void AddFormat(EncodingFormat format)
                => formats.Add(format);
        }

        public record struct ParameterInfo(EncodingFormat EncodingFormat, byte[]? Value)
        { }

        public class BindBuilder : IMessageBuilder<Bind>
        {
            private BindPayload Payload { get; set; }

            internal BindBuilder()
            {
                Payload = new BindPayload(string.Empty, string.Empty, new(), new AllResultsAsText());
            }

            internal BindBuilder(string destination, string source)
            {
                Payload = new BindPayload(destination, source, new(), new AllResultsAsText());
            }

            public BindBuilder WithParameter(EncodingFormat format, byte[]? value)
            {
                Payload.Parameters.Add(new(format, value));
                return this;
            }

            public BindBuilder WithAllResultsAsText()
            {
                Payload = Payload with { ResultsFormat = new AllResultsAsText() };
                return this;
            }

            public BindBuilder WithAllResultsAsBinary()
            {
                Payload = Payload with { ResultsFormat = new AllResultsAsBinary() };
                return this;
            }

            public BindBuilder WithResultFormat(EncodingFormat format)
            {
                if (Payload.ResultsFormat is not ResultsFormat)
                    Payload = Payload with { ResultsFormat = new ResultsFormat() };
                ((ResultsFormat)(Payload.ResultsFormat)).AddFormat(format);
                return this;
            }

            public Bind Build()
            {
                var msg = new Bind(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
