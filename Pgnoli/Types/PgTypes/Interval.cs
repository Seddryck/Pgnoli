using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Interval : PgType
    {
        public Interval()
            : base(1186, 16, 0) { }
    }
}
