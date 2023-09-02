using Pgnoli.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Frontend
{
    public class FrontendParser
    {
        public Message Parse(byte[] bytes, int start, out int length)
        {
            var message = Decode(bytes.Skip(start).ToArray());
            length = message.Read();
            return message;
        }

        protected Message Decode(byte[] bytes) 
        { 
            var code = Convert.ToChar(bytes[0]);
            return code switch
            {
                '\0'                            => new Handshake.Startup(bytes),
                Authentication.Password.Code    => new Authentication.Password(bytes),
                Query.Bind.Code                 => new Query.Bind(bytes),
                Query.Close.Code                => new Query.Close(bytes),
                Query.Describe.Code             => new Query.Describe(bytes),
                Query.Execute.Code              => new Query.Execute(bytes),
                Query.Parse.Code                => new Query.Parse(bytes),
                Query.Sync.Code                 => new Query.Sync(bytes),
                Query.Query.Code                => new Query.Query(bytes),
                Handshake.Terminate.Code        => new Handshake.Terminate(bytes),
                _ => throw new MessageUnexpectedCodeException(code, bytes),
            };
        }
    }
}
