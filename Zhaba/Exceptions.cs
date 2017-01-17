using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Zhaba
{
  /// <summary>
  /// Base exception thrown by the Zhaba system
  /// </summary>
  [Serializable]
  public class ZhabaException : Exception
  {
    #region .ctor

      public ZhabaException()
      {
      }

      public ZhabaException(string message)
        : base(message)
      {
      }

      public ZhabaException(string message, Exception inner)
        : base(message, inner)
      {
      }

      protected ZhabaException(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {

      }

    #endregion
  }

  /// <summary>
  /// Base exception thrown by the Zhaba.Web
  /// </summary>
  [Serializable]
  public class ZhabaWebException : ZhabaException
  {
    #region .ctor

      public ZhabaWebException()
      {
      }

      public ZhabaWebException(string message)
        : base(message)
      {
      }

      public ZhabaWebException(string message, Exception inner)
        : base(message, inner)
      {
      }

      protected ZhabaWebException(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
      }

    #endregion
  }


  /// <summary>
  /// Base exception thrown by the Zhaba.Data
  /// </summary>
  [Serializable]
  public class ZhabaDataException : ZhabaException
  {
    #region .ctor

      public ZhabaDataException()
      {
      }

      public ZhabaDataException(string message)
        : base(message)
      {
      }

      public ZhabaDataException(string message, Exception inner)
        : base(message, inner)
      {
      }

      protected ZhabaDataException(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
      }

    #endregion
  }
}

