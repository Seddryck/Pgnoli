using Pgnoli.Options.DateStyles;
using Pgnoli.Messages;
using Pgnoli.Messages.Frontend;
using Pgnoli.Messages.Frontend.Query;
using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing.Messages.Frontend
{
    public class FrontendParserTest
    {
        [Test]
        [TestCase("Frontend.Query.Bind.UnnamedPortal", typeof(Bind))]
        [TestCase("Frontend.Query.Close.UnnamedPortal", typeof(Close))]
        [TestCase("Frontend.Query.Describe.UnnamedPortal", typeof(Describe))]
        [TestCase("Frontend.Query.Execute.UnnamedPortal", typeof(Execute))]
        [TestCase("Frontend.Query.Query.To_timestamp", typeof(Pgnoli.Messages.Frontend.Query.Query))]
        [TestCase("Frontend.Query.Parse.To_timestamp", typeof(Parse))]
        [TestCase("Frontend.Query.Sync.Default", typeof(Sync))]
        public void Parse(string msgPath, Type expected) 
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read(msgPath);
            
            var parser = new FrontendParser();
            var msg = parser.Parse(bytes, 0, out var length);
            Assert.That(msg, Is.TypeOf(expected));
            Assert.That(length, Is.EqualTo(bytes.Length));
        }
    }
}
