using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Varchar : PgType
    {
        public Varchar()
            : base(1043, -1, 0) { }
    }
}
