using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.PgTypes
{
    internal class Time : PgType
    {
        public Time()
            : base(1083, 8, 0) { }
    }
}
