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
    }
}
