using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class NumericTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0-0-0-0-0-0-3")]
        [TestCase(1, "0-1-0-0-0-0-0-3-0-1")]
        [TestCase(-1, "0-1-0-0-64-0-0-3-0-1")]
        [TestCase(0.001, "0-1-255-255-0-0-0-3-0-10")]
        [TestCase(0.123d, "0-1-255-255-0-0-0-3-4-206")]
        [TestCase(0.1, "0-1-255-255-0-0-0-3-3-232")]
        [TestCase(1.1, "0-2-0-0-0-0-0-3-0-1-3-232")]
        [TestCase(10002, "0-2-0-1-0-0-0-3-0-1-0-2")]
        [TestCase(10002.1, "0-3-0-1-0-0-0-3-0-1-0-2-3-232")]
        public void Write_Text_Success(decimal value, string expected)
        {
            var handler = new DecimalTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + StringToBytes(expected).Length);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(StringToBytes(expected).Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0-0-0-0-0-0-3", 0)]
        [TestCase("0-1-0-0-0-0-0-3-0-1", 1)]
        [TestCase("0-1-0-0-64-0-0-3-0-1", -1)]
        [TestCase("0-1-255-255-0-0-0-3-0-10", 0.001)]
        [TestCase("0-1-255-255-0-0-0-3-4-206", 0.123)]
        [TestCase("0-1-255-255-0-0-0-3-3-232", 0.1)]
        [TestCase("0-2-0-0-0-0-0-3-0-1-3-232", 1.1)]
        [TestCase("0-3-0-1-0-0-0-3-0-1-0-2-3-232", 10002.1)]
        public void Read_Text_Success(string value, decimal expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new DecimalTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
