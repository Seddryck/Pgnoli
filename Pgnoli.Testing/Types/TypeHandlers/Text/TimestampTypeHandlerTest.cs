using Pgnoli.Options.DateStyles;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class TimestampTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("2000-01-01 00:00:00", "50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48")]
        [TestCase("1978-12-28 17:12:56", "49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54")]
        [TestCase("1978-12-28 17:12:56.123", "49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54-46-49-50-51")]
        [TestCase("1978-12-28 17:12:56.123456", "49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54")]
        public void Write_IsoYMD_Success(string value, string expected)
        {
            var handler = new TimestampTypeHandler(new IsoYMD());
            var buffer = new Buffer();
            buffer.Allocate(4 + value.Length);
            handler.Write(DateTime.Parse(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48", "2000-01-01 00:00:00")]
        [TestCase("49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54", "1978-12-28 17:12:56")]
        [TestCase("49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51", "1978-12-28 17:12:56.123")]
        [TestCase("49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54", "1978-12-28 17:12:56.123456")]
        public void Read_IsoYMD_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTypeHandler(new IsoYMD());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTime.Parse(expected)));
        }

        [Test]
        [TestCase("2000-01-01 00:00:00",        "48-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48")]
        [TestCase("1978-12-28 17:12:56",        "49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54")]
        [TestCase("1978-12-28 17:12:56.123",    "49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51")]
        [TestCase("1978-12-28 17:12:56.123456", "49-50-45-50-56-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54")]
        public void Write_IsoMDY_Success(string value, string expected)
        {
            var handler = new TimestampTypeHandler(new IsoMDY());
            var buffer = new Buffer();
            buffer.Allocate(4 + value.Length);
            handler.Write(DateTime.Parse(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("50-48-48-48-45-48-49-45-48-49-32-48-48-58-48-48-58-48-48", "2000-01-01 00:00:00")]
        [TestCase("49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54", "1978-12-28 17:12:56")]
        [TestCase("49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54-46-49-50-51", "1978-12-28 17:12:56.123")]
        [TestCase("49-57-55-56-45-49-50-45-50-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54", "1978-12-28 17:12:56.123456")]
        public void Read_IsoMDY_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTypeHandler(new IsoMDY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTime.Parse(expected)));
        }


        [Test]
        [TestCase("2000-01-01 00:00:00",        "48-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48")]
        [TestCase("1978-12-28 17:12:56",        "50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54")]
        [TestCase("1978-12-28 17:12:56.123",    "50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51")]
        [TestCase("1978-12-28 17:12:56.123456", "50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54")]
        public void Write_IsoDMY_Success(string value, string expected)
        {
            var handler = new TimestampTypeHandler(new IsoDMY());
            var buffer = new Buffer();
            buffer.Allocate(4 + value.Length);
            handler.Write(DateTime.Parse(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48-49-45-48-49-45-50-48-48-48-32-48-48-58-48-48-58-48-48", "2000-01-01 00:00:00")]
        [TestCase("50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54", "1978-12-28 17:12:56")]
        [TestCase("50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51", "1978-12-28 17:12:56.123")]
        [TestCase("50-56-45-49-50-45-49-57-55-56-32-49-55-58-49-50-58-53-54-46-49-50-51-52-53-54", "1978-12-28 17:12:56.123456")]
        public void Read_IsoDMY_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimestampTypeHandler(new IsoDMY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateTime.Parse(expected)));
        }
    }
}
