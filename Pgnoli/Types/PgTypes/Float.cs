using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Float : PgType
    {
        public Float()
            : base(701, 8, 0) { }
    }
}
