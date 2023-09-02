using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Handshake;

namespace Pgnoli.Testing.Messages.Backend.Handshake
{
    public class ReadyForQueryTest
    {
        [Test]
        public void Roundtrip_Idle_Success()
        {
            var msg = ReadyForQuery.Idle.Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(Convert.ToChar(bytes[5]), Is.EqualTo('I'));

            var roundtrip = new ReadyForQuery(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.State, Is.EqualTo(roundtrip.Payload.State));
            });
        }

        [Test]
        public void Read_Idle_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Handshake.ReadyForQuery.Idle");
            var msg = new ReadyForQuery(bytes);

            Assert.DoesNotThrow(() => msg.Read());
            Assert.That(msg.Payload.State, Is.EqualTo('I'));
        }
    }
}
