using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class DecimalTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "48")]
        [TestCase(1, "49")]
        [TestCase(0.1, "48-46-49")]
        [TestCase(-0.1, "45-48-46-49")]
        public void Write_Text_Success(decimal value, string expected)
        {
            var handler = new DecimalTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + value.ToString().Length);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.ToString().Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48", 0)]
        [TestCase("49", 1)]
        [TestCase("48-46-49", 0.1)]
        [TestCase("45-48-46-49", -0.1)]
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
