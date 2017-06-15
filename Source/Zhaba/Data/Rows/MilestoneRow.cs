﻿using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_milestone")]
  public class MilestoneRow : ZhabaRowProjectBase
  {
    public MilestoneRow() : base() { }
    public MilestoneRow(RowPKAction action) : base(action) { }


    [Field(required: true,
           kind: DataKind.Text,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           description: "Name",
           metadata: @"Placeholder='Name'")]
    public string Name { get; set; }

    [Field(maxLength: ZhabaDescription.MAX_LEN,
           kind: DataKind.Text,
           description: "Description",
           metadata: @"Placeholder='Description' ControlType='textarea'")]
    public string Description { get; set; }

    [Field(kind: DataKind.Date,
           description: "Start Date")]
    public DateTime Start_Date { get; set; }

    [Field(kind: DataKind.Date,
           description: "Plan Date")]
    public DateTime Plan_Date { get; set; }

    [Field(kind: DataKind.Date,
           description: "Completion Date")]
    public DateTime Complete_Date { get; set; }
  }
}
