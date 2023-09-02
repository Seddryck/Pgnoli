using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers
{
    internal abstract class BaseTypeHandler<T> : ITypeWriter<T>, ITypeReader<T>, ITypeHandler
    {
        public abstract T Read(ref Buffer buffer);
        public abstract void Write(T value, ref Buffer buffer);
        void ITypeWriter.Write(object value, ref Buffer buffer) => Write((T)value, ref buffer);
        object ITypeReader.Read(ref Buffer buffer) => Read(ref buffer)!;
    }
}
