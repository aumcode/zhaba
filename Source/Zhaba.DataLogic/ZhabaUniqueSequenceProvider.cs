using System;
using System.Linq;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data
{
  public class ZhabaSequenceProvider : IUniqueSequenceProvider
  {
    #region Nested

    private class SequenceInfo : ISequenceInfo
    {
      public SequenceInfo(string Name, ulong Value)
      {
        m_Name = Name;
        m_Value = Value;
      }

      private readonly string m_Name;
      private readonly ulong m_Value;

      public ulong    ApproximateCurrentValue { get { return m_Value; }}
      public uint     Era                     { get { return 0; }}
      public string   IssuerName              { get { return null; }}
      public DateTime IssueUTCDate            { get { throw new NotImplementedException(); }}
      public string   Name                    { get { return m_Name; }}
      public int      RemainingPreallocation  { get { throw new NotImplementedException(); }}
      public int      TotalPreallocation      { get { throw new NotImplementedException(); }}
    }

    #endregion

    // temporary solution: need to sync all ID generation processes
    private static readonly object m_Sync = new object();

    public string Name { get { return "ZhabaSequenceProvider"; }}

    public IEnumerable<string> SequenceScopeNames
    {
      get
      {
        var qry = QSequence.SequenceScopes();
        return ZApp.Data.CRUD.LoadOneRowset(qry)
                             .Select(r => r[0].AsString());
      }
    }

    public ulong GenerateOneSequenceID(string scopeName, string sequenceName, int blockSize = 0, ulong? vicinity = ulong.MaxValue, bool noLWM = false)
    {
      lock (m_Sync)
      {
        var x = SequenceScopeNames;
        using (var tx = ZApp.Data.CRUD.BeginTransaction())
        {
          var qry = QSequence.SequenceByName<SequenceRow>(scopeName, sequenceName);
          var sequence = ZApp.Data.CRUD.LoadRow(qry);
          if (sequence == null) sequence = new SequenceRow
          {
            Scope_Name = scopeName,
            Sequence_Name = sequenceName,
            Counter = 0
          };
          sequence.Counter++;

          tx.Upsert(sequence);
          tx.Commit();

          return sequence.Counter;
        }
      }
    }

    public IEnumerable<ISequenceInfo> GetSequenceInfos(string scopeName)
    {
      var qry = QSequence.SequencesByScopeName<SequenceRow>(scopeName);
      return ZApp.Data.CRUD.LoadEnumerable(qry)
                           .Select(s => new SequenceInfo(s.Scope_Name, s.Counter));
    }

    public ConsecutiveUniqueSequenceIDs TryGenerateManyConsecutiveSequenceIDs(string scopeName, string sequenceName, int idCount, ulong? vicinity = ulong.MaxValue, bool noLWM = false)
    {
      throw new NotImplementedException();
    }
  }
}