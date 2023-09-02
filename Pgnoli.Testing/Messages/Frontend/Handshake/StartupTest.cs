using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Handshake;

namespace Pgnoli.Testing.Messages.Frontend.Handshake
{
    public class StartupTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = Startup.Message("3.0", "root", "helloworld").WithOption("client_encoding", "UTF8").Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Handshake.Startup.Default")));
        }

        [Test]
        public void Read_Default_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Handshake.Startup.Default");
            var msg = new Startup(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
