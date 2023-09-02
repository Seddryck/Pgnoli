using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Text
{
    internal abstract class BaseTextTypeHandler<T> : BaseTypeHandler<T>, ITypeReader<T>, ITypeWriter<T>
    {
        public override T Read(ref Buffer buffer)
        {
            var size = buffer.ReadInt();
            var bytes = buffer.ReadBytes(size);
            return Parse(Encoding.UTF8.GetString(bytes));
        }

        protected abstract T Parse(string value);

        public override void Write(T value, ref Buffer buffer)
        {
            var str = Convert(value);
            buffer.WriteInt(str.Length);
            buffer.WriteString(str);
        }

        protected abstract string Convert(T value);
    }
}
