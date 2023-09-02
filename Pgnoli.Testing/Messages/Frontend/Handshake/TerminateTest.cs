using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Handshake;

namespace Pgnoli.Testing.Messages.Frontend.Handshake
{
    public class TerminateTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = Terminate.Message.Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Handshake.Terminate.Default")));
        }

        [Test]
        public void Read_UnnamedPortal_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Handshake.Terminate.Default");
            var msg = new Terminate(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
