using Castle.Components.DictionaryAdapter;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Binary
{
    public class BoolTypeHandlerTest
    {
        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public void Write_Text_Success(bool value, byte expected)
        {
            var handler = new BooleanTypeHandler();
            var buffer = new Buffer();
            buffer.Allocate(4 + 1);
            handler.Write(value, ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(1)));
            Assert.That(buffer.GetBytes()[4], Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void Read_Text_Success(byte value, bool expected)
        {
            var buffer = new Buffer(IntToBytes(1).Concat(new byte[] { value }).ToArray());

            var handler = new BooleanTypeHandler();
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
