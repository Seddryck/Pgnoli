using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Bool : PgType
    {
        public Bool()
            : base(16, 1, 0) { }
    }
}
