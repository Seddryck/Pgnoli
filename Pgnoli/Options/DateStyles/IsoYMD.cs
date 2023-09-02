using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pgnoli.Options.DateStyles
{
    internal class IsoYMD : Iso
    {
        public override string DateFormat => "yyyy/MM/dd";
    }
}
