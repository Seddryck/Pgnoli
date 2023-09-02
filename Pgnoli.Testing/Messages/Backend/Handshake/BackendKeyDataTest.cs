using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Handshake;

namespace Pgnoli.Testing.Messages.Backend.Handshake
{
    public class BackendKeyDataTest
    {
        [Test]
        public void Roundtrip_Ok_Success()
        {
            var msg = BackendKeyData.Message(1, 12345).Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.GreaterThanOrEqualTo('A').And.LessThanOrEqualTo('Z'));

            var roundtrip = new BackendKeyData(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload!.ProcessId, Is.EqualTo(roundtrip.Payload!.ProcessId));
                Assert.That(msg.Payload!.SecretKey, Is.EqualTo(roundtrip.Payload!.SecretKey));
            });
        }
    }
}
