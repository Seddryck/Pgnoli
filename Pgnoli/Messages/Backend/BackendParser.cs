using Pgnoli.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages.Backend
{
    internal class BackendParser
    {
        private readonly IMessageContext Context;

        public BackendParser(IMessageContext context)
            => Context = context;

        public Message Parse(byte[] bytes)
        {
            var message = Decode(bytes);
            message.Read();
            return message;
        }

        protected Message Decode(byte[] bytes) 
        { 
            var code = Convert.ToChar(bytes[0]);
            return code switch
            {
                Handshake.BackendKeyData.Code           => new Handshake.BackendKeyData(bytes),
                Handshake.ReadyForQuery.Code            => new Handshake.ReadyForQuery(bytes),
                Handshake.ParameterStatus.Code            => new Handshake.ParameterStatus(bytes),
                //Authentication.CleartextPassword.Code   => new Authentication.CleartextPassword(bytes),
                //Authentication.MD5Password.Code         => new Authentication.MD5Password(bytes),
                Authentication.Ok.Code                   => new Authentication.Ok(bytes),
                Query.BindComplete.Code                 => new Query.BindComplete(bytes),
                Query.CloseComplete.Code                => new Query.CloseComplete(bytes),
                Query.CommandComplete.Code              => new Query.CommandComplete(bytes),
                Query.DataRow.Code                      => new Query.DataRow(bytes, Context.FieldDescriptions, Context.TypeHandlerFactory),
                Query.ErrorResponse.Code                => new Query.ErrorResponse(bytes),
                Query.ParseComplete.Code                => new Query.ParseComplete(bytes),
                Query.RowDescription.Code               => new Query.RowDescription(bytes),
                _ => throw new ArgumentException(nameof(bytes)),
            };
        }
    }
}
