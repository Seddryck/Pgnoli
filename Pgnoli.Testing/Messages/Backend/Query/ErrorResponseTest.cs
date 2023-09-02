using Pgnoli.Messages;
using Pgnoli.Messages.Backend.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Messages.Backend.Query
{
    public class ErrorResponseTest
    {
        [Test]
        public void Roundtrip_Ok_Success()
        {
            var msg = ErrorResponse.Warning("SQL00001", "!Oups!").With('q', "SELECT @@version").Build();
            var bytes = msg.GetBytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(bytes, Has.Length.EqualTo(51));
                Assert.That(bytes[0], Is.EqualTo('E'));
                Assert.That(bytes[50], Is.EqualTo(0));
            });

            var roundtrip = new ErrorResponse(bytes);
            Assert.DoesNotThrow(() => roundtrip.Read());
            Assert.Multiple(() =>
            {
                Assert.That(roundtrip.Payload.Severity, Is.EqualTo(msg.Payload.Severity));
                Assert.That(roundtrip.Payload.SqlState, Is.EqualTo(msg.Payload.SqlState));
                Assert.That(roundtrip.Payload.Message, Is.EqualTo(msg.Payload.Message));
                Assert.That(roundtrip.Payload.OptionalMessages, Has.Count.EqualTo(msg.Payload.OptionalMessages.Count));
                foreach (var option in roundtrip.Payload.OptionalMessages)
                {
                    Assert.That(msg.Payload.OptionalMessages.ContainsKey(option.Key), Is.True);
                    Assert.That(roundtrip.Payload.OptionalMessages[option.Key], Is.EqualTo(msg.Payload.OptionalMessages[option.Key]));
                }
            });
        }

        [Test]
        public void Write_Default_Success()
        {
            var msg = ErrorResponse.Warning("SQL00001", "!Oups!").With('q', "SELECT @@version").Build();

            var bytes = msg.GetBytes();

            var reader = new ResourceBytesReader();
            Assert.That(bytes, Is.EqualTo(reader.Read("Backend.Query.ErrorResponse.Default")));
        }

        [Test]
        public void Read_Default_Success()
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read("Backend.Query.ErrorResponse.Default");
            var msg = new ErrorResponse(bytes);

            Assert.DoesNotThrow(() => msg.Read());
            Assert.That(msg.Payload.Severity, Is.EqualTo(ErrorSeverity.Warning));
            Assert.That(msg.Payload.SqlState, Is.EqualTo("SQL00001"));
            Assert.That(msg.Payload.Message, Is.EqualTo("!Oups!"));
            Assert.That(msg.Payload.OptionalMessages.ContainsKey('q'), Is.True);
            Assert.That(msg.Payload.OptionalMessages['q'], Is.EqualTo("SELECT @@version"));
        }
    }
}
