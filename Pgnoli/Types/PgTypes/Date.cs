using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Date : PgType
    {
        public Date()
            : base(1082, 4, 0) { }
    }
}
