using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Smallint : PgType
    {
        public Smallint()
            : base(21, 2, 0) { }
    }
}
