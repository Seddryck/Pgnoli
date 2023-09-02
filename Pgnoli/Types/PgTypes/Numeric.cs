using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Numeric : PgType
    {
        public Numeric()
            : base(1700, -1, 0) { }
    }
}
