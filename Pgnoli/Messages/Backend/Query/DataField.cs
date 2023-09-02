using Pgnoli.Types.TypeHandlers;
using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pgnoli.Messages.Backend.Query
{
    public record struct DataField(
        object Data
    )
    { }
}
