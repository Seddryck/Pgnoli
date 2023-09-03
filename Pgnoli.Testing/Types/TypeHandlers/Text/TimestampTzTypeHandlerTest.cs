using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json.Linq;
using Pgnoli.Options.DateStyles;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class TimestampTzTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("2000-01-01 00:00:00 +00:00", "50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48")]
        [TestCase("2000-01-01 00:00:00 +01:00", "50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49")]
        [TestCase("2000-01-01 00:00:00 -01:00", "50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49")]
        public void Write_IsoYMD_Success(string value, string expected)
        {
            var handler = new TimestampTzTypeHandler(new IsoYMD());
            var buffer = new Buffer();
            buffer.Allocate(4 + 29);
            var dt = DateTimeOffset.Parse(value);
            handler.Write(dt, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(29)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48", "2000-01-01 00:00:00 +00:00")]
        [TestCase("50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49", "2000-01-01 00:00:00 +01:00")]
        [TestCase("50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49", "2000-01-01 00:00:00 -01:00")]
        public void Read_IsoYMD_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTzTypeHandler(new IsoYMD());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTimeOffset.Parse(expected)));
        }

        [Test]
        [TestCase("2000-01-21 00:00:00 +00:00", "48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48")]
        [TestCase("2000-01-21 00:00:00 +01:00", "48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49")]
        [TestCase("2000-01-21 00:00:00 -01:00", "48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49")]
        public void Write_IsoMDY_Success(string value, string expected)
        {
            var handler = new TimestampTzTypeHandler(new IsoMDY());
            var buffer = new Buffer();
            buffer.Allocate(4 + 29);
            var dt = DateTimeOffset.Parse(value);
            handler.Write(dt, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(29)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48", "2000-01-21 00:00:00 +00:00")]
        [TestCase("48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49", "2000-01-21 00:00:00 +01:00")]
        [TestCase("48-49-45-50-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49", "2000-01-21 00:00:00 -01:00")]
        public void Read_IsoMDY_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTzTypeHandler(new IsoMDY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTimeOffset.Parse(expected)));
        }

        [Test]
        [TestCase("2000-01-21 00:00:00 +00:00", "50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48")]
        [TestCase("2000-01-21 00:00:00 +01:00", "50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49")]
        [TestCase("2000-01-21 00:00:00 -01:00", "50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49")]
        public void Write_IsoDMY_Success(string value, string expected)
        {
            var handler = new TimestampTzTypeHandler(new IsoDMY());
            var buffer = new Buffer();
            buffer.Allocate(4 + 29);
            var dt = DateTimeOffset.Parse(value);
            handler.Write(dt, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(29)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-48", "2000-01-21 00:00:00 +00:00")]
        [TestCase("50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-43-48-49", "2000-01-21 00:00:00 +01:00")]
        [TestCase("50-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48-46-48-48-48-48-48-48-45-48-49", "2000-01-21 00:00:00 -01:00")]
        public void Read_IsoDMY_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTzTypeHandler(new IsoDMY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTimeOffset.Parse(expected)));
        }
    }
}
