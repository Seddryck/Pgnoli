using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class TimestampTz : PgType
    {
        public TimestampTz()
            : base(1184, 8, 0) { }
    }
}
