using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Options.DateStyles;
using Pgnoli.Messages.Backend.Query;
using Pgnoli.Types.TypeHandlers;
using static Pgnoli.Messages.Backend.Query.FieldDescription;
using Pgnoli.Messages;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class DataRowTest
    {
        private static readonly TypeHandlerFactory TypeHandlers = new(new IsoYMD());

        [Test]
        public void Roundtrip_Ok_Success()
        {
            var descFactory = new FieldDescriptionFactory();
            var desc = new FieldDescription[]
            {
                descFactory.Instantiate<string>("Region", EncodingFormat.Binary),
                descFactory.Instantiate<DateTime>("Instant", EncodingFormat.Binary),
                descFactory.Instantiate<decimal>("Value", EncodingFormat.Text),
            };

            var msg = new DataRow.DataRowBuilder(desc, TypeHandlers)
                            .With("Flanders")
                            .With(new DateTime(2023, 12, 28, 7, 34, 56))
                            .With(-10.2533m)
                            .Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(bytes, Has.Length.GreaterThan(0));
                Assert.That(bytes[0], Is.GreaterThanOrEqualTo('A').And.LessThanOrEqualTo('Z'));
                Assert.That(bytes, Has.Length.GreaterThan(5));
                Assert.That(bytes, Has.Length.LessThan(255));
            });

            var roundtrip = new DataRow(bytes, desc, TypeHandlers);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(roundtrip.Payload!.Values, Has.Count.EqualTo(msg.Payload!.Values.Count));
                for (int i = 0; i < msg.Payload!.Values.Count; i++)
                    Assert.That(roundtrip.Payload.Values[i], Is.EqualTo(msg.Payload.Values[i]));
            });
        }

        [Test]
        public void Write_SingleInt_Success()
        {
            var descFactory = new FieldDescriptionFactory();
            var desc = new FieldDescription[]
            {
                descFactory.Instantiate<int>("Value", EncodingFormat.Binary)
            };

            var msg = new DataRow.DataRowBuilder(desc, TypeHandlers)
                            .With(1)
                            .Build();

            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.DataRow.SingleInt")));
        }

        [Test]
        public void Read_SingleInt_Success()
        {
            var descFactory = new FieldDescriptionFactory();
            var desc = new FieldDescription[]
            {
                descFactory.Instantiate<int>("Value", EncodingFormat.Binary)
            };

            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.DataRow.SingleInt");
            var msg = new DataRow(bytes, desc, TypeHandlers);

            Assert.DoesNotThrow(() => msg.Read());
            Assert.That(msg.Payload.Values, Has.Count.EqualTo(1));
            Assert.That(msg.Payload.Values[0], Is.EqualTo(1));
        }
    }
}
