﻿using System;

using NFX;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.Rows
{
  public abstract class ZhabaRow : AmorphousTypedRow
  {
    protected ZhabaRow() : base() { }

    /// <summary>
    /// Disable for security
    /// </summary>
    public override bool AmorphousDataEnabled { get { return false; } }

    /// <summary>
    /// Generates new ID properly scoping it and naming it for this row
    /// </summary>
    public virtual ulong GenerateNewID()
    {
      var sequenceName = this.GetType().Name.Replace("Row", string.Empty);
      return ZApp.Data.SequenceProvider.GenerateOneSequenceID(Consts.ZHABA_SCOPE, sequenceName);
    }
  }

  /// <summary>
  /// Specifies when ulong PK will be created
  /// </summary>
  public enum RowPKAction
  {
    /// <summary>
    /// GDID will be generated by ValidateAndPrepareForStore() when there are no validation errors
    /// </summary>
    Default = 0,

    /// <summary>
    /// Ulong D will be generated by ctor immediately, in this case if Validate fails then new ID may be wasted if row instance is discarded
    /// </summary>
    CtorGenerateNewID
  }

  /// <summary>
  /// Zhaba rows with ulong PK
  /// </summary>
  public abstract class ZhabaRowWithPK : ZhabaRow
  {
    protected ZhabaRowWithPK() : base() { }
    protected ZhabaRowWithPK(RowPKAction action)
    {
      if (action == RowPKAction.CtorGenerateNewID)
      {
        Counter = GenerateNewID();
        ApplyDefaultFieldValues();
      }
    }

    [Field(key: true, required: true, visible: false)]
    public ulong Counter { get; set; }

    public Exception ValidateAndPrepareForStore(string targetName = null)
    {
      if (targetName.IsNullOrWhiteSpace()) targetName = App.DataStore.TargetName;

      DoPrepareForStorePreValidate(targetName);

      var verror = this.Validate(targetName);
      if (verror != null) return verror;

      DoPrepareForStorePostValidate(targetName);

      if (Counter == 0)
        return new CRUDFieldValidationException(this, "Counter", "Counter PK Value is required in row " + GetType().Name);

      return null;
    }

    public override Exception ValidateField(string targetName, Schema.FieldDef fdef)
    {
      if (fdef.Name.EqualsIgnoreCase("GDID")) return null;//Skip GDID field validation
      return base.ValidateField(targetName, fdef);
    }


    /// <summary>
    /// Override to perform extra work before row gets validated before written to store.
    /// </summary>
    protected virtual void DoPrepareForStorePreValidate(string targetName)
    {

    }

    /// <summary>
    /// Override to perform extra work before row gets written to store after successfull validate.
    /// The default implementation generates GDID if it was not generated yet
    /// </summary>
    protected virtual void DoPrepareForStorePostValidate(string targetName)
    {
      if (this.Counter == 0) Counter = GenerateNewID();
    }
  }

  /// <summary>
  /// Zhaba rows with PK and In-Use
  /// </summary>
  public abstract class ZhabaRowWithPKAndInUse : ZhabaRowWithPK
  {
    protected ZhabaRowWithPKAndInUse():base(){}

    protected ZhabaRowWithPKAndInUse(RowPKAction action) : base(action)
    {
      if (action == RowPKAction.Default)
      {
        In_Use = true;
      }
    }

    [Field(required: true, description: "In use", dflt: true)]
    public bool In_Use { get; set; }
  }

  /// <summary>
  /// Zhaba rows with PK, In_Use and reference to project
  /// </summary>
  public abstract class ZhabaRowProjectBase : ZhabaRowWithPKAndInUse
  {
    protected ZhabaRowProjectBase() : base() {}

    protected ZhabaRowProjectBase(RowPKAction action) : base(action) {}

    [Field(required: true, nonUI: true)]
    public ulong C_Project { get; set; }
  }
}
