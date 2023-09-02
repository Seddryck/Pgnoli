using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Testing
{
    public class BufferTest
    {
        [Test]
        public void Allocate_Twice_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(10);
            Assert.Throws<BufferAlreadyAllocatedException>(() => buffer.Allocate(10));
        }

        [Test]
        public void ReadShort_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(1);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadShort());
        }

        [Test]
        public void ReadShort_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(2);
            Assert.DoesNotThrow(() => buffer.ReadShort());
        }

        [Test]
        public void ReadInt_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(3);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadInt());
        }

        [Test]
        public void ReadInt_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(4);
            Assert.DoesNotThrow(() => buffer.ReadInt());
        }

        [Test]
        public void ReadFixedSizeString_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(1);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadFixedSizeString(2));
        }

        [Test]
        public void ReadFixedSizeString_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(2);
            Assert.DoesNotThrow(() => buffer.ReadFixedSizeString(2));
        }

        [Test]
        public void ReadBytes_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(1);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadBytes(2));
        }

        [Test]
        public void ReadBytes_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(2);
            Assert.DoesNotThrow(() => buffer.ReadBytes(2));
        }

        [Test]
        public void ReadByte_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(0);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadByte());
        }

        [Test]
        public void ReadByte_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(1);
            Assert.DoesNotThrow(() => buffer.ReadByte());
        }

        [Test]
        public void ReadAsciiChar_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(0);
            Assert.Throws<BufferOverflowException>(() => buffer.ReadAsciiChar());
        }

        [Test]
        public void ReadAsciiChar_LongEnoughBuffer_DoesNotThrowException()
        {
            var buffer = new Buffer();
            buffer.Allocate(1);
            Assert.DoesNotThrow(() => buffer.ReadAsciiChar());
        }

        [Test]
        public void Peek_NotLongEnoughBuffer_ThrowsException()
        {
            var buffer = new Buffer();
            buffer.Allocate(0);
            Assert.Throws<BufferOverflowException>(() => buffer.Peek());
        }
    }
}
