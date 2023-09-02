using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class TimeTz : PgType
    {
        public TimeTz()
            : base(1266, 12, 0) { }
    }
}
