using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class SmallintTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0")]
        [TestCase(1, "0-1")]
        [TestCase(255, "0-255")]
        [TestCase(256, "1-0")]
        [TestCase(257, "1-1")]
        [TestCase(short.MaxValue, "127-255")]
        [TestCase(short.MinValue, "128-0")]
        [TestCase(-1, "255-255")]
        public void Write_Text_Success(short value, string expected)
        {
            var handler = new ShortTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 2);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(2)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0", 0)]
        [TestCase("0-1", 1)]
        [TestCase("0-255", 255)]
        [TestCase("1-0", 256)]
        [TestCase("1-1", 257)]
        [TestCase("127-255", short.MaxValue)]
        [TestCase("128-0", short.MinValue)]
        [TestCase("255-255", -1)]
        public void Read_Text_Success(string value, int expected)
        {
            var buffer = new Buffer(IntToBytes(2).Concat(StringToBytes(value)).ToArray());

            var handler = new ShortTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
