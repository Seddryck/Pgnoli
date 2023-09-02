using Pgnoli.Options.DateStyles;
using Pgnoli.Messages;
using Pgnoli.Messages.Backend;
using Pgnoli.Messages.Backend.Query;
using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Pgnoli.Types.PgTypes;

namespace Pgnoli.Testing.Messages.Backend
{
    public class BackendParserTest
    {
        private class StubContext : IMessageContext
        {
            public FieldDescription[] FieldDescriptions => new[] { new FieldDescription("value", 0, 0, new Int(), EncodingFormat.Binary)};
            public TypeHandlerFactory TypeHandlerFactory => new (new IsoYMD());
        }

        [Test]
        [TestCase("Backend.Query.BindComplete.Default", typeof(BindComplete))]
        [TestCase("Backend.Query.CloseComplete.Default", typeof(CloseComplete))]
        [TestCase("Backend.Query.CommandComplete.SelectRowCount", typeof(CommandComplete))]
        [TestCase("Backend.Query.DataRow.SingleInt", typeof(DataRow))]
        [TestCase("Backend.Query.ErrorResponse.Default", typeof(ErrorResponse))]
        [TestCase("Backend.Query.ParseComplete.Default", typeof(ParseComplete))]
        [TestCase("Backend.Query.RowDescription.SingleInt", typeof(RowDescription))]
        public void Parse(string msgPath, Type expected) 
        {
            var reader = new ResourceBytesReader();
            var bytes = reader.Read(msgPath);
            
            var parser = new BackendParser(new StubContext());
            var msg = parser.Parse(bytes);
            Assert.That(msg, Is.TypeOf(expected));
        }
    }
}
