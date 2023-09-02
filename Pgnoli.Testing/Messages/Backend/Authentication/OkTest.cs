using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Authentication;

namespace Pgnoli.Testing.Messages.Backend.Authentication
{
    public class OkTest
    {
        [Test]
        public void Roundtrip_Ok_Success()
        {
            var msg = Ok.Message.Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.GreaterThanOrEqualTo('A').And.LessThanOrEqualTo('Z'));

            var roundtrip = new Ok(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
        }
    }
}
