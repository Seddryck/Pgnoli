using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json.Linq;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class TimestampTzTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] LongToBytes(long value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        [Test]
        [TestCase("2000-01-01 00:00:00 +00:00", 0)]
        [TestCase("2000-01-01 00:00:01 +00:00", 1_000_000)]
        [TestCase("1999-12-31 23:59:59 +00:00", -1_000_000)]
        [TestCase("2000-01-01 00:00:00 +01:00", -3_600_000_000)]
        [TestCase("2000-01-01 00:00:00 -01:00", +3_600_000_000)]
        public void Write_ShiftToEpoch_Success(DateTimeOffset value, long microseconds)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);

            var handler = new TimestampTzTypeHandler();
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(8)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(LongToBytes(microseconds)));
        }

        [Test]
        [TestCase("0-0-137-199-97-230-154-128", "2004-10-19 10:23:54+02")]
        public void Read_Binary_Success(string binary, DateTimeOffset expected)
        {
            var buffer = new Buffer(IntToBytes(8).Concat(StringToBytes(binary)).ToArray());

            var handler = new TimestampTzTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result, Is.EqualTo(expected.ToUniversalTime()));
        }

        [Test]
        [TestCase(0, "2000-01-01 00:00:00 +00:00")]
        [TestCase(1_000_000, "2000-01-01 00:00:01 +00:00")]
        [TestCase(-1_000_000, "1999-12-31 23:59:59 +00:00")]
        [TestCase(-3_600_000_000, "2000-01-01 00:00:00 +01:00")]
        [TestCase(+3_600_000_000, "2000-01-01 00:00:00 -01:00")]
        public void Read_ShiftToEpoch_Success(long microseconds, DateTimeOffset expected)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);
            buffer.WriteInt(8);
            buffer.WriteLong(microseconds);
            buffer.Reset();

            var handler = new TimestampTzTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result, Is.EqualTo(expected.ToUniversalTime()));
        }
    }
}
