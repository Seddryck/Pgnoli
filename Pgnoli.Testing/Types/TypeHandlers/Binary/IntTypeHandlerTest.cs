using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class IntTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0-0-0")]
        [TestCase(1, "0-0-0-1")]
        [TestCase(255, "0-0-0-255")]
        [TestCase(256, "0-0-1-0")]
        [TestCase(257, "0-0-1-1")]
        [TestCase(int.MaxValue, "127-255-255-255")]
        [TestCase(int.MinValue, "128-0-0-0")]
        [TestCase(-1, "255-255-255-255")]
        public void Write_Text_Success(int value, string expected)
        {
            var handler = new IntTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 4);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(4)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0-0-0", 0)]
        [TestCase("0-0-0-1", 1)]
        [TestCase("0-0-0-255", 255)]
        [TestCase("0-0-1-0", 256)]
        [TestCase("0-0-1-1", 257)]
        [TestCase("127-255-255-255", int.MaxValue)]
        [TestCase("128-0-0-0", int.MinValue)]
        [TestCase("255-255-255-255", -1)]
        public void Read_Text_Success(string value, int expected)
        {
            var buffer = new Buffer(IntToBytes(4).Concat(StringToBytes(value)).ToArray());

            var handler = new IntTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
