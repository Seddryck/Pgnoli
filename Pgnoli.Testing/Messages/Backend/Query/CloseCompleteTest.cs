using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Query;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class CloseCompleteTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = CloseComplete.Message.Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.CloseComplete.Default")));
        }

        [Test]
        public void Read_Default_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.CloseComplete.Default");
            var msg = new CloseComplete(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
