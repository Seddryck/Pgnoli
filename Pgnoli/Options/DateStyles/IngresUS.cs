using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options
{
    internal class IngresUS : Ingres
    {
        public override string DateFormat => "MM/dd/yyyy";
    }
}
