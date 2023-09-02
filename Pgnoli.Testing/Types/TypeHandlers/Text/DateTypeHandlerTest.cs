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
    public class DateTypeHandlerTest
    {
        private static byte[] StringToBytes(string value)
            => value.Split("-").Select(byte.Parse).ToArray();

        private static byte[] IntToBytes(int value)
            => BitConverter.GetBytes(value).Reverse().ToArray();

        [Test]
        [TestCase("2000-01-01", "50-48-48-48-45-48-49-45-48-49")]
        [TestCase("1978-12-28", "49-57-55-56-45-49-50-45-50-56")]
        public void Write_IsoYMD_Success(DateTime value, string expected)
        {
            var handler = new DateTypeHandler(new IsoYMD());
            var buffer = new Buffer();
            buffer.Allocate(4 + 10);
            handler.Write(DateOnly.FromDateTime(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(10)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("2000-01-01", "48-49-45-48-49-45-50-48-48-48")]
        [TestCase("1978-12-28", "49-50-45-50-56-45-49-57-55-56")]
        public void Write_IsoMDY_Success(DateTime value, string expected)
        {
            var handler = new DateTypeHandler(new IsoMDY());
            var buffer = new Buffer();
            buffer.Allocate(4 + 10);
            handler.Write(DateOnly.FromDateTime(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(10)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("2000-01-01", "48-49-45-48-49-45-50-48-48-48")]
        [TestCase("1978-12-28", "50-56-45-49-50-45-49-57-55-56")]
        public void Write_IsoDMY_Success(DateTime value, string expected)
        {
            var handler = new DateTypeHandler(new IsoDMY());
            var buffer = new Buffer();
            buffer.Allocate(4 + 10);
            handler.Write(DateOnly.FromDateTime(value), ref buffer);

            Assert.That(buffer.GetBytes()[..4], Is.EqualTo(IntToBytes(10)));
            Assert.That(buffer.GetBytes()[4..], Is.EqualTo(StringToBytes(expected)));
        }

        [Test]
        [TestCase("50-48-48-48-45-48-49-45-48-49", "2000-01-01")]
        [TestCase("49-57-55-56-45-49-50-45-50-56", "1978-12-28")]
        public void Read_IsoYMD_Success(string value, DateTime expected)
        {
            var buffer = new Buffer(IntToBytes(10).Concat(StringToBytes(value)).ToArray());

            var handler = new DateTypeHandler(new IsoYMD());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateOnly.FromDateTime(expected)));
        }


        [Test]
        [TestCase("48-49-45-48-49-45-50-48-48-48", "2000-01-01")]
        [TestCase("49-50-45-50-56-45-49-57-55-56", "1978-12-28")]
        public void Read_IsoMDY_Success(string value, DateTime expected)
        {
            var buffer = new Buffer(IntToBytes(10).Concat(StringToBytes(value)).ToArray());

            var handler = new DateTypeHandler(new IsoMDY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateOnly.FromDateTime(expected)));
        }


        [Test]
        [TestCase("48-49-45-48-49-45-50-48-48-48", "2000-01-01")]
        [TestCase("50-56-45-49-50-45-49-57-55-56", "1978-12-28")]
        public void Read_IsoDMY_Success(string value, DateTime expected)
        {
            var buffer = new Buffer(IntToBytes(10).Concat(StringToBytes(value)).ToArray());

            var handler = new DateTypeHandler(new IsoDMY());
            var result = handler.Read(ref buffer);

            Assert.That(buffer.IsEnd(), Is.True);
            Assert.That(result, Is.EqualTo(DateOnly.FromDateTime(expected)));
        }
    }
}
