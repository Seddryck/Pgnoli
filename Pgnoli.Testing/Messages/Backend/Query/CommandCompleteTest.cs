using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages.Backend.Query;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class CommandCompleteTest
    {
        private static CommandComplete.CommandCompleteBuilder[] BuilderCases
            => new[]
            {
                CommandComplete.Select(1978),
                CommandComplete.Delete(1978),
                CommandComplete.Merge(1978),
                CommandComplete.Move(1978),
                CommandComplete.Update(1978),
                CommandComplete.Fetch(1978),
                CommandComplete.Copy(1978),
            };

        [Test]
        [TestCaseSource(nameof(BuilderCases))]
        public void Roundtrip_SelectRowCount_Success(CommandComplete.CommandCompleteBuilder builder)
        {
            var msg = builder.Build();
            Assert.That(msg.Payload.RowCount, Is.EqualTo(1978));

            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.EqualTo('C'));

            var roundtrip = new CommandComplete(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.Tag, Is.EqualTo(roundtrip.Payload.Tag));
                Assert.That(msg.Payload.RowCount, Is.EqualTo(roundtrip.Payload.RowCount));
            });
        }

        [Test]
        public void Roundtrip_InsertNoRowCount_Success()
        {
            var msg = CommandComplete.Insert.Build();
            Assert.That(msg.Payload.RowCount, Is.EqualTo(0));

            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes, Has.Length.GreaterThan(0));
            Assert.That(bytes[0], Is.GreaterThanOrEqualTo('A').And.LessThanOrEqualTo('Z'));

            var roundtrip = new CommandComplete(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(msg.Payload.Tag, Is.EqualTo(roundtrip.Payload.Tag));
                Assert.That(msg.Payload.RowCount, Is.EqualTo(roundtrip.Payload.RowCount));
            });
        }

        [Test]
        public void Write_SelectRowCount_Success()
        {
            var msg = CommandComplete.Select(1).Build();
            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.CommandComplete.SelectRowCount")));
        }

        [Test]
        public void Read_SelectRowCount_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.CommandComplete.SelectRowCount");
            var msg = new CommandComplete(bytes);

            Assert.DoesNotThrow(() => msg.Read());
            Assert.That(msg.Payload.Tag, Is.EqualTo("SELECT"));
            Assert.That(msg.Payload.RowCount, Is.EqualTo(1));
        }
    }
}
