using Pgnoli.Messages;
using Pgnoli.Messages.Backend.Handshake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Messages
{
    public class CodeMessageTest
    {
        private class StubMessage : CodeMessage
        {
            public const char Code = '=';

            public StubMessage(byte[] bytes)
                : base(Code, bytes) { }

            protected override int GetPayloadLength() => 0;
            protected internal override void ReadPayload(Buffer buffer) { }
            protected internal override void WritePayload(Buffer buffer) { }
        }

        private class StubBrokenMessage : StubMessage
        {
            public StubBrokenMessage(byte[] bytes)
                : base(bytes) { }

            protected override int GetPayloadLength() => 1;
        }

        [Test]
        public void Read_ValidBytes_DoesNotThrow()
        {
            var bytes = new Byte[] { Convert.ToByte(StubMessage.Code), 0, 0, 0, 4 };
            var msg = new StubMessage(bytes);
            Assert.DoesNotThrow(() => msg.Read());
        }

        [Test]
        public void Read_UnexpectedCode_Throws()
        {
            var bytes = new Byte[] { (byte)(Convert.ToByte(StubMessage.Code)-1), 0, 0, 0, 4 };
            var msg = new StubMessage(bytes);
            Assert.Throws<MessageUnexpectedCodeException>(() => msg.Read());
        }

        [Test]
        public void Read_UnexpectedLength_Throws()
        {
            var bytes = new Byte[] { Convert.ToByte(StubMessage.Code), 0, 0, 0, 20 };
            var msg = new StubMessage(bytes);
            Assert.Throws<MessageUnexpectedLengthException>(() => msg.Read());
        }

        [Test]
        public void Read_MessageNotFullyConsumedException_Throws()
        {
            var bytes = new Byte[] { Convert.ToByte(StubMessage.Code), 0, 0, 0, 5, 1 };
            var msg = new StubBrokenMessage(bytes);
            Assert.Throws<MessageNotFullyConsumedException>(() => msg.Read());
        }

        [Test]
        public void Read_EmptyBuffer_Throws()
        {
            var bytes = Array.Empty<byte>();
            var msg = new StubMessage(bytes);
            Assert.Throws<BufferEmptyException>(() => msg.Read());
        }
    }
}
