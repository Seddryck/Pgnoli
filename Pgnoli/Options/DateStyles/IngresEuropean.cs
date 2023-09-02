using Pgnoli.Options.DateStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options
{
    internal class IngresEuropean : Ingres
    {
        public override string DateFormat => "dd/MM/yyyy";
    }
}
