using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Int : PgType
    {
        public Int()
            : base(23, 4, 0) { }
    }
}
