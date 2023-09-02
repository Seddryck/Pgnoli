using System;
using System.Reflection;

namespace Pgnoli;

public abstract class PgnoliException : ApplicationException
{
    public PgnoliException(string message)
         : base(message)
    { }

    public PgnoliException(string message, Exception innerException)
         : base(message, innerException)
    { }
}