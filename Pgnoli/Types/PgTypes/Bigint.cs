using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Bigint : PgType
    {
        public Bigint()
            : base(20, 8, 0) { }
    }
}
