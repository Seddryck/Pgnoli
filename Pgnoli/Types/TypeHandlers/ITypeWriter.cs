using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers
{
    public interface ITypeWriter
    {
        void Write(object value, ref Buffer buffer);
    }

    internal interface ITypeWriter<T> : ITypeWriter
    {
        void Write(T value, ref Buffer buffer);
    }
}
