﻿using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using NFX;
using NFX.Wave;
using NFX.Serialization.JSON;
using System.Collections.Generic;

namespace Zhaba.Data.Forms
{
  public class IssueAssignForm :ProjectFormBase
  {
    #region .ctor
    public IssueAssignForm() { }
    public IssueAssignForm(ProjectRow project, IssueRow issue, ulong? counter) : base(project)
    {
      m_Issue = issue;
      if (counter.HasValue) 
      {
        FormMode = FormMode.Edit;
        var row = ZApp.Data.CRUD.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value));
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Project");
        this.RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else 
      {
        Open_TS = DateTime.UtcNow;
      }
    }
    #endregion

    [NonSerialized]
    private IssueRow m_Issue;

    #region Fields
    [Field(typeof(IssueAssignRow))]
    public ulong C_Issue { get; set; }
    [Field(typeof(IssueAssignRow))]
    public string C_User { get; set; }
    [Field(typeof(IssueAssignRow))]
    public DateTime Open_TS { get; set; }
    [Field(typeof(IssueAssignRow))]
    public DateTime? Close_TS { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_Close_Meeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_Open_Meeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public string Note { get; set; }

    public IssueRow Issue { get { return m_Issue; } }
    public ulong IssueID { get { return m_Issue.Counter; } }

    #endregion

    #region Public

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var user = fdef.Name.EqualsIgnoreCase("C_User");
      JSONDataMap result = null;
      if (user)
      {
        var users = ZApp.Data.CRUD.LoadEnumerable<UserForPM>(QUser.FindAllActiveUserAndNotAssignedOnDate<UserForPM>(Issue.Counter, DateTime.UtcNow));
        result = new JSONDataMap();
        foreach (UserForPM item in users)
        {
          result.Add(item.Counter.ToString(), item.FullName);
        }
      }
      return result;
    }

    #endregion

    #region Protected
    protected override Exception DoSave(out object saveResult)
    {
      return ZApp.Data.Issue.WriteIssueAssignForm(this, out saveResult);
    }

    public void ____SetIssue(IssueRow issue)
    {
      m_Issue = issue;
    }

   
    #endregion
  }
}
