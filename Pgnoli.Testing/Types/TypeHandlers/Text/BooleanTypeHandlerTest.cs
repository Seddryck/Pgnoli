using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class BooleanTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(true, 't')]
        [TestCase(false, 'f')]
        public void Write_Text_Success(bool value, char expected)
        {
            var handler = new BooleanTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 1);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(1)));
            Assert.That(buffer.GetBytes()[4], Is.EqualTo(Convert.ToByte(expected)));
        }

        [Test]
        [TestCase('t', true)]
        [TestCase('f', false)]
        public void Read_Text_Success(char value, bool expected)
        {
            var buffer = new Buffer(IntToBytes(1).Concat(new byte[] { Convert.ToByte(value) }).ToArray());

            var handler = new BooleanTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
