using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Messages;
using Pgnoli.Messages.Backend.Query;
using static Pgnoli.Messages.Backend.Query.FieldDescription;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class RowDescriptionTest
    {
        [Test]
        public void Roundtrip_Ok_Success()
        {
            var msg = new RowDescription.RowDescriptionBuilder()
                            .With<string>("Facet", EncodingFormat.Text)
                            .With<DateTime>("Instant", EncodingFormat.Binary)
                            .With<decimal>("Measurement", EncodingFormat.Text)
                            .Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(bytes, Has.Length.GreaterThan(0));
                Assert.That(bytes[0], Is.GreaterThanOrEqualTo('A').And.LessThanOrEqualTo('Z'));
            });

            var roundtrip = new RowDescription(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(roundtrip.Payload!.Fields, Has.Count.EqualTo(msg.Payload!.Fields.Count));
                foreach (var field in roundtrip.Payload!.Fields)
                {
                    Assert.That(msg.Payload!.Fields.Any(x => field.Name == x.Name));
                    var msgField = msg.Payload!.Fields.Single(x => field.Name == x.Name);
                    Assert.Multiple(() =>
                    {
                        Assert.That(field.TableObjectId, Is.EqualTo(msgField.TableObjectId));
                        Assert.That(field.ColumnAttributeNumber, Is.EqualTo(msgField.ColumnAttributeNumber));
                        Assert.That(field.PgType.DataTypeObjectId, Is.EqualTo(msgField.PgType.DataTypeObjectId));
                        Assert.That(field.PgType.DataTypeSize, Is.EqualTo(msgField.PgType.DataTypeSize));
                        Assert.That(field.PgType.TypeModifier, Is.EqualTo(msgField.PgType.TypeModifier));
                        Assert.That(field.FormatCode, Is.EqualTo(msgField.FormatCode));
                    });
                }
            });
        }
    }
}