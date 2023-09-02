using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal abstract class BaseTimestampTypeHandler<T, U> : BaseBinaryTypeHandler<T>
    {
        public readonly DateTime Postgres_Epoch_Date = new(2000, 1, 1);

        protected BaseBinaryTypeHandler<U> UnderlyingTypeHandler { get; set; }

        public BaseTimestampTypeHandler(BaseBinaryTypeHandler<U> underlyingTypeHandler)
            => UnderlyingTypeHandler = underlyingTypeHandler;

        public byte[] GetUnderlyingBytes(U value)
            => UnderlyingTypeHandler.GetBytes(value);

        public U ReadUnderlyingValue(ref Buffer buffer)
            => ReadUnderlyingValue<U>(ref buffer);
    }
}
