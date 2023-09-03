using Pgnoli.Options.DateStyles;
using Pgnoli.Types.TypeHandlers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Types.TypeHandlers.Text
{
    public class TextTypeHandlerFactoryTest
    {
        [Test]
        public void Instantiate_CSharpType_CorrectTypeHandler()
        {
            var factory = new TextTypeHandlerFactory(new IsoDMY());
            Assert.Multiple(() =>
            {
                Assert.That(factory.Instantiate<string>(), Is.TypeOf<StringTypeHandler>());
                Assert.That(factory.Instantiate<char>(), Is.TypeOf<CharTypeHandler>());
                Assert.That(factory.Instantiate<bool>(), Is.TypeOf<BooleanTypeHandler>());
                Assert.That(factory.Instantiate<short>(), Is.TypeOf<ShortTypeHandler>());
                Assert.That(factory.Instantiate<int>(), Is.TypeOf<IntTypeHandler>());
                Assert.That(factory.Instantiate<long>(), Is.TypeOf<LongTypeHandler>());
                Assert.That(factory.Instantiate<float>(), Is.TypeOf<SingleTypeHandler>());
                Assert.That(factory.Instantiate<double>(), Is.TypeOf<DoubleTypeHandler>());
                Assert.That(factory.Instantiate<decimal>(), Is.TypeOf<DecimalTypeHandler>());
                Assert.That(factory.Instantiate<DateTime>(), Is.TypeOf<TimestampTypeHandler>());
                Assert.That(factory.Instantiate<DateOnly>(), Is.TypeOf<DateTypeHandler>());
                Assert.That(factory.Instantiate<TimeOnly>(), Is.TypeOf<TimeTypeHandler>());
                Assert.That(factory.Instantiate<DateTimeOffset>(), Is.TypeOf<TimestampTzTypeHandler>());
            });
        }
    }
}
