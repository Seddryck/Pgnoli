using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class ShortTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(0, "48")]
        [TestCase(1, "49")]
        [TestCase(255, "50-53-53")]
        [TestCase(256, "50-53-54")]
        [TestCase(257, "50-53-55")]
        [TestCase(1000, "49-48-48-48")]
        [TestCase(-1, "45-49")]
        public void Write_Text_Success(short value, string expected)
        {
            var handler = new ShortTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + value.ToString().Length);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.ToString().Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48", 0)]
        [TestCase("49", 1)]
        [TestCase("50-53-53", 255)]
        [TestCase("45-49", -1)]
        public void Read_Text_Success(string value, short expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new ShortTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
