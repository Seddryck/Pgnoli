using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class BpChar : PgType
    {
        public BpChar()
            : base(18, 1, 0) { }
    }
}
