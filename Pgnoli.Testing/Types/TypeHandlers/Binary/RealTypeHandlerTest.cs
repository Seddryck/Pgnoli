using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class RealTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "0-0-0-0")]
        [TestCase(1, "63-128-0-0")]
        [TestCase(0.1f, "61-204-204-205")]
        [TestCase(-0.1f, "189-204-204-205")]
        public void Write_Text_Success(float value, string expected)
        {
            var handler = new SingleTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 4);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(4)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("0-0-0-0", 0)]
        [TestCase("63-128-0-0", 1)]
        [TestCase("61-204-204-205", 0.1f)]
        [TestCase("189-204-204-205", -0.1f)]
        public void Read_Text_Success(string value, float expected)
        {
            var buffer = new Buffer(IntToBytes(4).Concat(StringToBytes(value)).ToArray());

            var handler = new SingleTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
