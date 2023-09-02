using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    internal interface IMessageBuilder<T>
    {
        T Build();
    }
}
