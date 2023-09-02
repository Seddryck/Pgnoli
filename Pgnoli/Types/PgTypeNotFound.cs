using Pgnoli.Types.PgTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types
{
    internal class DataTypeObjectIdNotFound : PgnoliException
    {
        public DataTypeObjectIdNotFound(int dataTypeObjectId, short dataTypeSize, int typeModifier)
            : base($"There is no PgType defined with the following parameters dataTypeObjectId '{dataTypeObjectId}', dataTypeSize '{dataTypeSize}', typeModifier '{typeModifier}'") { }
    }

    internal class PgTypeNotFound : PgnoliException
    {
        public PgTypeNotFound(PgType[] types, int dataTypeObjectId, short dataTypeSize, int typeModifier)
            : base($"There are existing PgTypes with dataTypeObjectId '{dataTypeObjectId}' which are '{string.Join("', '", types.Select(x => x.GetType().Name))}' not none of them has dataTypeSize '{dataTypeSize}', typeModifier '{typeModifier}'") { }
    }
}
