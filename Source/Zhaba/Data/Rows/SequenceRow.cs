using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_sequence")]
  public class SequenceRow : TypedRow
  {
    [Field(key: true,
           required: true,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN)]
    public string Scope_Name { get; set; }

    [Field(key: true,
           required: true,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN)]
    public string Sequence_Name { get; set; }

    [Field(required: true)]
    public ulong Counter { get; set; }
  }
}
