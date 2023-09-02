using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers
{
    public interface ITypeReader
    {
        object Read(ref Buffer buffer);
    }

    internal interface ITypeReader<T> : ITypeReader
    {
        new T Read(ref Buffer buffer);
    }
}
