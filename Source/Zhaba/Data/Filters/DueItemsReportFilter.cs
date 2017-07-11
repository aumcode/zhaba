using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.Forms;

namespace Zhaba.Data.Filters
{
  public class DueItemsReportFilter : ZhabaFilterForm
  {
    #region Nested

    private class DiagramPoint : TypedRow
    {
      [Field] public DateTime date { get; set; }
      [Field] public int Percent { get; set; }
    }
    
    private class DueItemsReportFilterRow : TypedRow
    {
      [Field] public ulong C_Project { get; set; }
      [Field] public ulong C_Issue { get; set; }
      [Field] public string ProjectName { get; set; }
      [Field] public string IssueName { get; set; }
      [Field] public DateTime Start_Date { get; set; }
      [Field] public DateTime Due_Date { get; set; }
      [Field] public string Users { get; set; }
      [Field] public DiagramPoint[] Diagram { get; set; }
    }

    #endregion

    [Field]
    public ulong? C_Project { get; set; }
    [Field]
    public DateTime? AsOf { get; set; }

    
    protected override Exception DoSave(out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      
      //todo  Здесь будет город сад
        
      return result;
    }
    
  }
}