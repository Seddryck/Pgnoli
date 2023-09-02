using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    public abstract class MessageException : PgnoliException
    {
        public MessageException(string message)
            : base(message) { }
    }

    public class MessageMismatchCodeException : MessageException
    {
        public MessageMismatchCodeException(Type MessageType, char expected, char actual)
            : base($"When reading a message of type '{MessageType.GetType().Name}', the expected code is '{expected}' but the code is '{actual}'.") { }
    }

    public class MessageUnexpectedCodeException : MessageException
    {
        public MessageUnexpectedCodeException(char code, byte[] bytes)
            : base($"A message with code '{code}' was decoded but this code can't be associated to a type of message. The content of the message was '{bytes}'") { }
    }

    public class MessageUnexpectedLengthException : MessageException
    {
        public MessageUnexpectedLengthException(Type MessageType, int expected, int actual)
            : base($"When reading a message of type '{MessageType.GetType().Name}', the expected length was '{expected}' but the actual length of the buffer is '{actual}'. Buffer length cannot be less than expected legth of the message.") { }
    }

    public class MessageNotFullyConsumedException : MessageException
    {
        public MessageNotFullyConsumedException(Type MessageType, int unreadBytes)
            : base($"When reading a message of type '{MessageType.GetType().Name}', the buffer wasn't read until the end. '{unreadBytes}' were not read.") { }
    }
}
