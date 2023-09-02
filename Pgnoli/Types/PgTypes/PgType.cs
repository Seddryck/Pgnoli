using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    public abstract class PgType
    {
        public readonly int DataTypeObjectId;
        public readonly short DataTypeSize;
        public readonly int TypeModifier;

        public PgType(int dataTypeObjectId, short dataTypeSize, int typeModifier)
            => (DataTypeObjectId,  DataTypeSize, TypeModifier) = (dataTypeObjectId, dataTypeSize, typeModifier);
    }
}
