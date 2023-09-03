using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Handshake;

namespace Pgnoli.Testing.Messages.Backend.Handshake
{
    public class ParameterStatusDataTest
    {
        [Test]
        public void Write_ClientEncoding_Success()
        {
            var msg = ParameterStatus.ClientEncoding("UTF8").Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Handshake.ParameterStatus.ClientEncoding")));
        }

        [Test]
        public void Read_ClientEncoding_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Handshake.ParameterStatus.ClientEncoding");
            var msg = new ParameterStatus(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }

        private static ParameterStatus.ParameterStatusBuilder[] BuilderCases()
            => new[]
            {
                ParameterStatus.ServerVersion("3.0"),
                ParameterStatus.ClientEncoding("UTF8"),
                ParameterStatus.ServerEncoding("UTF8"),
                ParameterStatus.WithIntegerDateTimes(),
                ParameterStatus.WithoutIntegerDateTimes(),
                ParameterStatus.Status("Key1", "Value1")
            };

        [Test]
        [TestCaseSource(nameof(BuilderCases))]
        public void Roundtrip_Ok_Success(ParameterStatus.ParameterStatusBuilder builder)
        {
            var msg = builder.Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.EqualTo('S'));

            var roundtrip = new ParameterStatus(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.Key, Is.EqualTo(roundtrip.Payload.Key));
                Assert.That(msg.Payload.Value, Is.EqualTo(roundtrip.Payload.Value));
            });
        }
    }
}
