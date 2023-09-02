using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    public class UnexpectedMessageCodeException : PgnoliException
    {
        public UnexpectedMessageCodeException(char code, byte[] bytes)
            : base($"A message with code '{code}' was decoded but this code can't be associated to a type of message. The content of the message was '{bytes}'") { }
    }
}
