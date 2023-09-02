using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pgnoli.Options.DateStyles
{
    internal class IsoDMY : Iso
    {
        public override string DateFormat => "dd/MM/yyyy";
    }
}
