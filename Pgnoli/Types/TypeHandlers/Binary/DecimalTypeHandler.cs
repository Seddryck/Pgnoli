using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pgnoli.Types.TypeHandlers.Binary
{
    internal class DecimalTypeHandler : BaseBinaryTypeHandler<decimal>
    {
        const short SignPositive = 0;
        const short SignNegative = 64 * 256;

        private readonly short Precision = 3;

        public override byte[] GetBytes(decimal value)
        {
            var str = Math.Abs(value).ToString(CultureInfo.InvariantCulture.NumberFormat);
            var tokens = str.Split('.');
            var integerStr = tokens[0] == "0" ? string.Empty : tokens[0];
            var decimalStr = tokens.Length == 2 ? tokens[1] : string.Empty;

            var array = new List<short>();
            var integerChunks = Chunk(string.Concat(integerStr.Reverse()), 4).Reverse().ToArray();
            foreach (var chunk in integerChunks)
                array.Add(Convert.ToInt16(string.Concat(chunk.Reverse())));

            var decimalChunks = Chunk(decimalStr, 4).ToArray();
            foreach (var chunk in decimalChunks)
                array.Add(Convert.ToInt16(chunk.PadRight(4, '0')));

            var size = 8 + array.Count * 2;
            var buffer = new Buffer((byte[])Array.CreateInstance(typeof(byte), size));

            buffer.WriteShort((short)array.Count);
            buffer.WriteShort((short)(integerChunks.Length != 0 ? integerChunks.Length - 1 : value == 0 ? 0 : -1));
            buffer.WriteShort(value >= 0 ? SignPositive : SignNegative);
            buffer.WriteShort(Precision);

            foreach (var chunk in array)
                buffer.WriteShort(chunk);

            return buffer.GetBytes();
        }

        public override decimal Read(ref Buffer buffer)
        {
            var size = buffer.ReadInt();
            var capacity = buffer.ReadShort();
            if (size != 8 + capacity * 2)
                throw new ArgumentOutOfRangeException();

            var weight = buffer.ReadShort();
            var sign = buffer.ReadShort() == SignPositive ? 1 : -1;
            var precision = buffer.ReadShort();

            var value = 0d;
            for (int i = weight; i >= 0 && capacity != 0; i--)
                value += buffer.ReadShort() * Math.Pow(10_000, i);

            for (int i = -1; Math.Abs(i) < capacity - weight; i--)
                value += buffer.ReadShort() * Math.Pow(10_000, i);

            value *= sign;
            return Convert.ToDecimal(Math.Round(value, precision));
        }

        private IEnumerable<string> Chunk(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                var subStr = str.Substring(i, Math.Min(chunkSize, str.Length - i));
                yield return subStr;
            }
        }
    }
}