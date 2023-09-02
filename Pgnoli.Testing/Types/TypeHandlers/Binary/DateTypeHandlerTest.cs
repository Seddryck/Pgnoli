using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class DateTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        private static byte[] LongToBytes(long value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("2000-01-01", 0)]
        [TestCase("2000-01-02", 1)]
        [TestCase("1999-12-31", -1)]
        public void Write_ShiftToEpoch_Success(DateTime value, int days)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 4);

            var handler = new DateTypeHandler();
            handler.Write(DateOnly.FromDateTime(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(4)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(IntToBytes(days)));
        }

        [Test]
        [TestCase(0, "2000-01-01")]
        [TestCase(1, "2000-01-02")]
        [TestCase(-1, "1999-12-31")]
        public void Read_ShiftToEpoch_Success(int days, DateTime expected)
        {
            var buffer = new Buffer();
            buffer.Allocate(4 + 4);
            buffer.WriteInt(4);
            buffer.WriteInt(days);
            buffer.Reset();

            var handler = new DateTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(result, Is.EqualTo(DateOnly.FromDateTime(expected)));
        }
    }
}
