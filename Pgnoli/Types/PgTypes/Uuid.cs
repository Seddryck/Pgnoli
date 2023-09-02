using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Uuid : PgType
    {
        public Uuid()
            : base(27, 16, 0) { }
    }
}
