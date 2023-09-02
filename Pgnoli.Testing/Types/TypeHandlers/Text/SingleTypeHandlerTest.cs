using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class SingleTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0f, "48")]
        [TestCase(1f, "49")]
        [TestCase(0.1f, "48-46-49")]
        [TestCase(-0.1f, "45-48-46-49")]
        public void Write_Text_Success(float value, string expected)
        {
            var handler = new SingleTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + value.ToString().Length);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.ToString().Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48", 0f)]
        [TestCase("49", 1f)]
        [TestCase("48-46-49", 0.1f)]
        [TestCase("45-48-46-49", -0.1f)]
        public void Read_Text_Success(string value, float expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new SingleTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
