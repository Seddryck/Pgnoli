using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Query;

namespace Pgnoli.Testing.Messages.Frontend.Query
{
    public class CloseTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = Close.UnnamedPortal.Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Query.Close.UnnamedPortal")));
        }

        [Test]
        public void Read_UnnamedPortal_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Query.Close.UnnamedPortal");
            var msg = new Close(bytes);

            Assert.DoesNotThrow(() => msg.Read());
            Assert.That(msg.Payload.PortalType, Is.EqualTo(PortalType.Portal));
            Assert.That(msg.Payload.Name, Is.EqualTo(string.Empty));
        }

        private static Close.CloseBuilder[] BuilderCases
            => new[]
            {
                Close.Portal("the_name"),
                Close.PreparedStatement("the_name"),
                Close.UnnamedPortal,
                Close.UnnamedPreparedStatement
            };

        [Test]
        [TestCaseSource(nameof(BuilderCases))]
        public void Roundtrip_Close_Success(Close.CloseBuilder builder)
        {
            var msg = builder.Build();

            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.EqualTo('C'));

            var roundtrip = new Close(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.Name, Is.EqualTo(roundtrip.Payload.Name));
                Assert.That(msg.Payload.PortalType, Is.EqualTo(roundtrip.Payload.PortalType));
            });
        }
    }
}
