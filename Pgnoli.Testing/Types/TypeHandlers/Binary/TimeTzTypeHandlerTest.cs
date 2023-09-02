using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class TimeTzTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] LongToBytes(long value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("00:00:00 +00:00", 0, 0)]
        [TestCase("00:00:01 +00:00", 1_000_000, 0)]
        [TestCase("00:01:00 +00:00", 60_000_000, 0)]
        [TestCase("00:00:01 -01:00", 1_000_000, 3600)]
        [TestCase("00:00:01 +01:00", 1_000_000, -3600)]
        public void Write_ShiftToEpoch_Success(DateTimeOffset value, long milliseconds, int offset)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8 + 4);

            var handler = new TimeTzTypeHandler();
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(12)));
            Assert.That(buffer.GetBytes()[4..12], Is.EqualTo(LongToBytes(milliseconds)));
            Assert.That(buffer.GetBytes()[12..], Is.EqualTo(IntToBytes(offset)));
        }

        [Test]
        [TestCase(0, 0, "00:00:00 +00:00")]
        [TestCase(1_000_000, 0, "00:00:01 +00:00")]
        [TestCase(60_000_000, 0, "00:01:00 +00:00")]
        [TestCase(1_000_000, 3600, "00:00:01 -01:00")]
        [TestCase(1_000_000, -3600, "00:00:01 +01:00")]
        public void Read_ShiftToEpoch_Success(long milliseconds, int offset, DateTimeOffset expected)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8 + 4);
            buffer.WriteInt(12);
            buffer.WriteLong(milliseconds);
            buffer.WriteInt(offset);
            buffer.Reset();

            var handler = new TimeTzTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result.TimeOfDay, Is.EqualTo(expected.TimeOfDay));
            Assert.That(result.Offset, Is.EqualTo(expected.Offset));
        }
    }
}
