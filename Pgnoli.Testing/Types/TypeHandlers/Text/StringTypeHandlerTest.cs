using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class StringTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("Belgium", "66-101-108-103-105-117-109")]
        [TestCase("R2D2", "82-50-68-50")]
        [TestCase("Hi Dad!", "72-105-32-68-97-100-33")]
        public void Write_Text_Success(string value, string expected)
        {
            var handler = new StringTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + value.Length);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("66-101-108-103-105-117-109", "Belgium")]
        [TestCase("82-50-68-50", "R2D2")]
        [TestCase("72-105-32-68-97-100-33", "Hi Dad!")]
        public void Read_Text_Success(string value, string expected)
        {
            var content = StringToBytes(value);
            var buffer = new Buffer(IntToBytes(content.Length).Concat(content).ToArray());

            var handler = new StringTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
