using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class BigintTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0-0-0-0-0-0-0")]
        [TestCase(1, "0-0-0-0-0-0-0-1")]
        [TestCase(255, "0-0-0-0-0-0-0-255")]
        [TestCase(256, "0-0-0-0-0-0-1-0")]
        [TestCase(257, "0-0-0-0-0-0-1-1")]
        [TestCase(long.MaxValue, "127-255-255-255-255-255-255-255")]
        [TestCase(long.MinValue, "128-0-0-0-0-0-0-0")]
        [TestCase(-1, "255-255-255-255-255-255-255-255")]
        public void Write_Text_Success(long value, string expected)
        {
            var handler = new LongTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(8)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0-0-0-0-0-0-0", 0)]
        [TestCase("0-0-0-0-0-0-0-1", 1)]
        [TestCase("0-0-0-0-0-0-0-255", 255)]
        [TestCase("0-0-0-0-0-0-1-0", 256)]
        [TestCase("0-0-0-0-0-0-1-1", 257)]
        [TestCase("127-255-255-255-255-255-255-255", long.MaxValue)]
        [TestCase("128-0-0-0-0-0-0-0", long.MinValue)]
        [TestCase("255-255-255-255-255-255-255-255", -1)]
        public void Read_Text_Success(string value, long expected)
        {
            var buffer = new Buffer(IntToBytes(8).Concat(StringToBytes(value)).ToArray());

            var handler = new LongTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
