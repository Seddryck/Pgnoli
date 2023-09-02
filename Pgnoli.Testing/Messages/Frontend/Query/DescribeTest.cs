using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Query;

namespace Pgnoli.Testing.Messages.Frontend.Query
{
    public class DescribeTest
    {
        [Test]
        public void Write_UnnamedPortal_Success()
        {
            var msg = Describe.Portal.Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Query.Describe.UnnamedPortal")));
        }

        [Test]
        public void Read_UnnamedPortal_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Query.Describe.UnnamedPortal");
            var msg = new Describe(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }

        [Test]
        public void Roundtrip_NamedPortal_Success()
        {
            var msg = Describe.Portal.Named("myName").Build();
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.PortalType, Is.EqualTo(PortalType.Portal));
                Assert.That(msg.Payload.Name, Is.EqualTo("myName"));
            });
            var bytes = msg.GetBytes();

            var roundtrip = new Describe(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.PortalType, Is.EqualTo(roundtrip.Payload.PortalType));
                Assert.That(msg.Payload.Name, Is.EqualTo(roundtrip.Payload.Name));
            });
        }
    }
}
