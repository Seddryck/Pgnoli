using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Timestamp : PgType
    {
        public Timestamp()
            : base(1114, 8, 0) { }
    }
}
