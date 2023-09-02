using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Query;

namespace Pgnoli.Testing.Messages.Frontend.Query
{
    public class BindTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = Bind.UnnamedPortal.WithAllResultsAsBinary().Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Query.Bind.UnnamedPortal")));
        }

        [Test]
        public void Read_UnnamedPortal_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Query.Bind.UnnamedPortal");
            var msg = new Bind(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
