using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class TimestampTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] LongToBytes(long value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("2000-01-01", 0)]
        [TestCase("2000-01-01 00:00:01", 1_000_000)]
        [TestCase("1999-12-31 23:59:59", -1_000_000)]
        public void Write_ShiftToEpoch_Success(DateTime value, long microseconds)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);

            var handler = new TimestampTypeHandler();
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(8)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(LongToBytes(microseconds)));
        }

        [Test]
        [TestCase(0, "2000-01-01")]
        [TestCase(1_000_000, "2000-01-01 00:00:01")]
        [TestCase(-1_000_000, "1999-12-31 23:59:59")]
        public void Read_ShiftToEpoch_Success(long microseconds, DateTime expected)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);
            buffer.WriteInt(8);
            buffer.WriteLong(microseconds);
            buffer.Reset();

            var handler = new TimestampTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
