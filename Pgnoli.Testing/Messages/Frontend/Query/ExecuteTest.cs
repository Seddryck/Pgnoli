using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Query;

namespace Pgnoli.Testing.Messages.Frontend.Query
{
    public class ExecuteTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = Execute.UnnamedPortal(0).Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Query.Execute.UnnamedPortal")));
        }

        [Test]
        public void Read_UnnamedPortal_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Query.Execute.UnnamedPortal");
            var msg = new Execute(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
