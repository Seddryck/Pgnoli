using Pgnoli.Types.PgTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Pgnoli.Messages.Backend.Query.FieldDescription;

namespace Pgnoli.Messages.Backend.Query
{
    public record struct FieldDescription(
        string Name,
        int TableObjectId,
        short ColumnAttributeNumber,
        PgType PgType,
        EncodingFormat FormatCode
   )
    {
        /// <summary>
        /// Return the length of this field 
        /// </summary>
        /// <remarks>
        /// 4 (int tableObjectId) + 2 (short columnAttributeNumber) + 4 (int dataTypeObjectId) + 2 (short dataTypeSize) + 4 (int typeModifier) + 2 (short formatCode)
        /// 4 + 2 + 4 + 2 + 4 + 2 = 18
        /// Add name length, plus 1 for null terminator '\0'
        /// </remarks>
        public readonly int Length
            => 18 + Name.Length + 1;

        public static FieldDescription Varchar(string name, EncodingFormat format)
            => new(name, 0, 0, new Varchar(), format);

        public static FieldDescription BpChar(string name, EncodingFormat format)
            => new(name, 0, 0, new BpChar(), format);

        public static FieldDescription Bool(string name, EncodingFormat format)
            => new (name, 0, 0, new Bool(), format);

        public static FieldDescription Smallint(string name, EncodingFormat format)
            => new (name, 0, 0, new Smallint(), format);

        public static FieldDescription Int(string name, EncodingFormat format)
            => new (name, 0, 0, new Int(), format);

        public static FieldDescription Bigint(string name, EncodingFormat format)
            => new (name, 0, 0, new Bigint(), format);

        public static FieldDescription Real(string name, EncodingFormat format)
            => new (name, 0, 0, new Real(), format);

        public static FieldDescription Float(string name, EncodingFormat format)
            => new (name, 0, 0, new Float(), format);

        public static FieldDescription Numeric(string name, EncodingFormat format)
            => new (name, 0, 0, new Numeric(), format);

        public static FieldDescription Date(string name, EncodingFormat format)
            => new (name, 0, 0, new Date(), format);

        public static FieldDescription Time(string name, EncodingFormat format)
            => new (name, 0, 0, new Time(), format);

        public static FieldDescription TimeTz(string name, EncodingFormat format)
            => new (name, 0, 0, new TimeTz(), format);

        public static FieldDescription Timestamp(string name, EncodingFormat format)
            => new(name, 0, 0, new Timestamp(), format);

        public static FieldDescription TimestampTz(string name, EncodingFormat format)
            => new(name, 0, 0, new TimestampTz(), format);

        public static FieldDescription Interval(string name, EncodingFormat format)
            => new(name, 0, 0, new Interval(), format);
    }
}
