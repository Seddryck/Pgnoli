using Pgnoli.Types.PgTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types
{
    internal class PgTypeFactory
    {
        private readonly PgType[] ExistingTypes = new PgType[]
        {
            new Bool(),
            new Smallint(),new Int(), new Bigint(),
            new Real(), new Float(),
            new Numeric(),
            new Date(), new Time(), new TimeTz(), new Timestamp(), new TimestampTz(), new Interval(),
            new BpChar(), new Varchar(),
            new Uuid(),
        };

        public PgType Instantiate(int dataTypeObjectId, short dataTypeSize, int typeModifier)
            => ExistingTypes.FirstOrDefault(x => x.DataTypeObjectId == dataTypeObjectId
                                                    && x.DataTypeSize == dataTypeSize
                                                    && x.TypeModifier == typeModifier)
                ?? throw (ExistingTypes.Any(x => x.DataTypeObjectId == dataTypeObjectId)
                    ? new PgTypeNotFound(ExistingTypes.Where(x => x.DataTypeObjectId == dataTypeObjectId).ToArray(), dataTypeObjectId, dataTypeSize, typeModifier)
                    : new DataTypeObjectIdNotFound(dataTypeObjectId, dataTypeSize, typeModifier));
    }
}
