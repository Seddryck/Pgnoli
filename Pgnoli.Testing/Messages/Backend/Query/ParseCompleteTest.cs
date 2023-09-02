using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Query;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class ParseCompleteTest
    {
        [Test]
        public void Write_Default_Success()
        {
            var msg = new ParseComplete();
            msg.Write();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.ParseComplete.Default")));
        }

        [Test]
        public void Read_Default_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.ParseComplete.Default");
            var msg = new ParseComplete(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
