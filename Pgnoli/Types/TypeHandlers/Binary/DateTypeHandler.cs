using Pgnoli.Types.TypeHandlers.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class DateTypeHandler : BaseTimestampTypeHandler<DateOnly, int>
    {
        public DateTypeHandler()
            : base(new IntTypeHandler()) { }

        public override byte[] GetBytes(DateOnly value)
            => GetUnderlyingBytes(value.ToDateTime(new TimeOnly(0, 0, 0)).Subtract(Postgres_Epoch_Date).Days);

        public override DateOnly Read(ref Buffer buffer)
        {
            if (buffer.ReadInt() != Unsafe.SizeOf<int>())
                throw new InvalidOperationException();

            var value = ReadUnderlyingValue(ref buffer);
            return DateOnly.FromDateTime(Postgres_Epoch_Date.AddDays(value));
        }
    }
}
