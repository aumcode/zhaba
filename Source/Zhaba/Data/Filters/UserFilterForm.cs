using System;
using NFX.ApplicationModel;
using NFX.DataAccess.CRUD;
using Zhaba.Data.Rows;
using NFX.Serialization.JSON;
using NFX;

namespace Zhaba.Data.Filters
{
  public class UserFilterForm : TypedRow
  {
    [Field(required: false, maxLength: 50)]
    public string Role { get; set; }

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      //if (fdef.Name.EqualsIgnoreCase("Role")) return ZApp.Data.Lookup("SQL.Lookup.Roles");

      return null;
    }
  }
}
