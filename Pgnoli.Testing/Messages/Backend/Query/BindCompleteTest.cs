using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Query;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class BindCompleteTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = BindComplete.Message.Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.BindComplete.Default")));
        }

        [Test]
        public void Read_Default_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.BindComplete.Default");
            var msg = new BindComplete(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
