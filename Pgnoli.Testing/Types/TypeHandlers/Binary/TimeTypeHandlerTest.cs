using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class TimeTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] LongToBytes(long value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("00:00:00", 0)]
        [TestCase("00:00:01", 1_000_000)]
        [TestCase("00:01:00", 60_000_000)]
        public void Write_ShiftToEpoch_Success(string value, long milliseconds)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);

            var handler = new TimeTypeHandler();
            handler.Write(TimeOnly.Parse(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(8)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(LongToBytes(milliseconds)));
        }

        [Test]
        [TestCase(0, "00:00:00")]
        [TestCase(1_000_000, "00:00:01")]
        [TestCase(60_000_000, "00:01:00")]
        public void Read_ShiftToEpoch_Success(long milliseconds, string expected)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);
            buffer.WriteInt(8);
            buffer.WriteLong(milliseconds);
            buffer.Reset();

            var handler = new TimeTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result, Is.EqualTo(TimeOnly.Parse(expected)));
        }
    }
}
