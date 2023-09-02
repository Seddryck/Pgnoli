using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Options.DateStyles
{
    public class DateStyleFactory
    {
        public IDateStyle Instantiate(string value)
        {
            var tokens = value.Split(',');
            var style = tokens[0].Trim();
            string format = string.Empty;
            if (tokens.Length==2)
                format = tokens[1].Trim();

            if (style == "Ingres" && format == string.Empty)
                format = "US";
            if (style == "ISO" && format == string.Empty)
                format = "MDY";

            style = format == string.Empty ? style : $"{style}, {format}";
            return style switch
            {
                "German" => new German(),
                "Posgres" => new Postgres(),
                "Ingres, European" => new IngresEuropean(),
                "Ingres, US" => new IngresUS(),
                "Iso, YMD" => new IsoYMD(),
                "Iso, DMY" => new IsoDMY(),
                "Iso, MDY" => new IsoMDY(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
