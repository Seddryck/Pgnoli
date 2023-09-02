using Pgnoli.Messages.Frontend.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend.Handshake
{
    public class Startup : Message
    {
        public StartupPayload Payload { get; private set; }

        internal Startup(byte[] bytes)
            : base(bytes) { }

        internal Startup(StartupPayload payload)
            : base() { Payload = payload; }

        protected override int GetPayloadLength()
            => 2 + 2
                    + Payload.Options.Keys.Select(x => x.Length + 1).Sum()
                    + Payload.Options.Values.Select(x => x.Length + 1).Sum()
                + 1;

        protected internal override void WritePayload(Buffer buffer)
        {
            buffer.WriteShort(Payload.Major);
            buffer.WriteShort(Payload.Minor);
            foreach (var option in Payload.Options)
            {
                buffer.WriteAsciiStringNullTerminator(option.Key);
                buffer.WriteAsciiStringNullTerminator(option.Value);
            }
            buffer.WriteByte(0);
        }

        protected internal override void ReadPayload(Buffer buffer)
        {
            var major = buffer.ReadShort();
            var minor = buffer.ReadShort();
            Payload = new StartupPayload(major, minor, new());

            if (!Buffer.IsEnd())
            {
                while (Buffer.Peek() != 0)
                {
                    var key = Buffer.ReadStringUntilNullTerminator();
                    var value = Buffer.ReadStringUntilNullTerminator();
                    Payload.Options.Add(key, value);
                }
                if (buffer.ReadByte() != 0)
                    throw new ArgumentException(nameof(Buffer));
            }
        }

        public static StartupBuilder Message(string user, string db) => new(user, db);
        public static StartupBuilder Message(string version, string user, string db) => new(version, user, db);

        public record struct StartupPayload(short Major, short Minor, Dictionary<string, string> Options)
        { }

        public class StartupBuilder : IMessageBuilder<Startup>
        {
            private StartupPayload Payload { get; set; }

            internal StartupBuilder(string user, string db)
                : this("3.0", user, db) { }

            internal StartupBuilder(string version, string user, string db)
            {
                var (major, minor) = (short.Parse(version.Split('.')[0]), short.Parse(version.Split('.')[1]));
                Payload = new StartupPayload(major, minor, new());
                Payload.Options.Add("user", user);
                Payload.Options.Add("database", db);
            }

            public StartupBuilder WithOption(string key, string value)
            {
                Payload.Options.Add(key, value);
                return this;
            }

            public Startup Build()
            {
                var msg = new Startup(Payload);
                msg.Write();
                return msg;
            }
        }
    }
}
