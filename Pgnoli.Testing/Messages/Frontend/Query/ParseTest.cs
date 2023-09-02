using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Frontend.Query;

namespace Pgnoli.Testing.Messages.Frontend.Query
{
    public class ParseTest
    {
        [Test]
        public void Write_To_timestamp_Success()
        {
            var msg = Parse.Sql("select to_timestamp('2022-12-10 15:18:16+7', 'YYYY-MM-DD HH24:MI:SSTZH')::timestamptz").Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Frontend.Query.Parse.To_timestamp")));
        }

        [Test]
        public void Read_To_timestamp_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Frontend.Query.Parse.To_timestamp");
            var msg = new Parse(bytes);

            Assert.DoesNotThrow(() => msg.Read());
        }
    }
}
