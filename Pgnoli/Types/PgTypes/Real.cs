using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Real : PgType
    {
        public Real()
            : base(700, 4, 0) { }
    }
}
