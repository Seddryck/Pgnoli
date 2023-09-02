using Pgnoli.Messages.Backend.Query;
using Pgnoli.Types.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Messages
{
    public interface IMessageContext
    {
        FieldDescription[] FieldDescriptions { get; }  
        TypeHandlerFactory TypeHandlerFactory { get; }
    }
}
