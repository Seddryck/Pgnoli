using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli
{
    public abstract class BufferPgnoliException : PgnoliException
    {
        protected BufferPgnoliException(string message)
            : base(message) { }
    }
    
    public class BufferNotAllocatedException : BufferPgnoliException
    {
        public BufferNotAllocatedException()
            : base($"The buffer is not allocated. You should call the method '{nameof(Buffer.Allocate)}' before this operation.") { }
    }

    public class BufferAlreadyAllocatedException : BufferPgnoliException
    {
        public BufferAlreadyAllocatedException()
            : base($"The buffer is already allocated. You cannot allocate it twice.") { }
    }

    public class BufferOverflowException : BufferPgnoliException
    {
        public BufferOverflowException(int size, int position, int bytesCount)
            : base($"The operation tries to read or write after the end of the buffer. The buffer has a total length of '{size}' and is positionned at the index '{position}' but the code was trying to read or write '{bytesCount}' bytes.") { }
    }

    public class BufferEmptyException : BufferPgnoliException
    {
        public BufferEmptyException()
            : base($"The operation tries to read a buffer but this buffer is empty (length equal to 0).") { }
    }

    public class BufferUnexpectedCharException : BufferPgnoliException
    {
        public BufferUnexpectedCharException(char unexpectedChar)
            : base($"The operation tries to read or write an Ascii char but is reaching the char '{unexpectedChar}'.") { }
    }
}
