using Castle.Components.DictionaryAdapter;
using Pgnoli.Options.DateStyles;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class TimeTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("00:00:00", "48-48-58-48-48-58-48-48")]
        [TestCase("17:12:56", "49-55-58-49-50-58-53-54")]
        [TestCase("17:12:56.123", "49-55-58-49-50-58-53-54-46-49-50-51")]
        [TestCase("17:12:56.123456", "49-55-58-49-50-58-53-54-46-49-50-51-52-53-54")]
        public void Write_IsoYMD_Success(string value, string expected)
        {
            var handler = new TimeTypeHandler(new IsoYMD());
            var buffer = new Buffer();
            buffer.Allocate(4 + value.Length);
            handler.Write(TimeOnly.Parse(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(value.Length)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("48-48-58-48-48-58-48-48", "00:00:00")]
        [TestCase("49-55-58-49-50-58-53-54", "17:12:56")]
        [TestCase("49-55-58-49-50-58-53-54-46-49-50-51", "17:12:56.123")]
        [TestCase("49-55-58-49-50-58-53-54-46-49-50-51-52-53-54", "17:12:56.123456")]
        public void Read_IsoYMD_Success(string value, string expected)
        {
            var buffer = new Buffer(IntToBytes(StringToBytes(value).Length).Concat(StringToBytes(value)).ToArray());

            var handler = new TimeTypeHandler(new IsoYMD());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(TimeOnly.Parse(expected)));
        }
    }
}
