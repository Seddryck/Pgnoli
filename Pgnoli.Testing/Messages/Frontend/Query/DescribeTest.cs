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
            var msg = Describe.UnnamedPortal.Build();
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
            Assert.That(msg.Payload.Name, Is.EqualTo(string.Empty));
            Assert.That(msg.Payload.PortalType, Is.EqualTo(PortalType.Portal));
        }

        private static Describe.DescribeBuilder[] BuilderCases
            => new[]
            {
                Describe.Portal("the_name"),
                Describe.PreparedStatement("the_name"),
                Describe.UnnamedPortal,
                Describe.UnnamedPreparedStatement
            };

        [Test]
        [TestCaseSource(nameof(BuilderCases))]
        public void Roundtrip_Close_Success(Describe.DescribeBuilder builder)
        {
            var msg = builder.Build();

            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.EqualTo('D'));

            var roundtrip = new Describe(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.Name, Is.EqualTo(roundtrip.Payload.Name));
                Assert.That(msg.Payload.PortalType, Is.EqualTo(roundtrip.Payload.PortalType));
            });
        }
    }
}
