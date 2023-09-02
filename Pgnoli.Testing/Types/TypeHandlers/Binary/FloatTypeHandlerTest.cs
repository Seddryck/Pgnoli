using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class FloatTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0-0-0-0-0-0-0")]
        [TestCase(1, "63-240-0-0-0-0-0-0")]
        [TestCase(0.1, "63-185-153-153-153-153-153-154")]
        [TestCase(-0.1, "191-185-153-153-153-153-153-154")]
        public void Write_Text_Success(double value, string expected)
        {
            var handler = new DoubleTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 8);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(8)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0-0-0-0-0-0-0", 0)]
        [TestCase("63-240-0-0-0-0-0-0", 1)]
        [TestCase("63-185-153-153-153-153-153-154", 0.1)]
        [TestCase("191-185-153-153-153-153-153-154", -0.1)]
        public void Read_Text_Success(string value, double expected)
        {
            var buffer = new Buffer(IntToBytes(8).Concat(StringToBytes(value)).ToArray());

            var handler = new DoubleTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
