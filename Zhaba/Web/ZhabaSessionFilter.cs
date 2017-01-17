using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

using NFX;
using NFX.Environment;
using NFX.Web;
using NFX.Wave;
using NFX.Wave.Filters;


using Zhaba.Security;
using NFX.Security;
using NFX.Serialization.JSON;

namespace Zhaba.Web
{
  /// <summary>
  /// Provides session management for Zhaba-specific sessions
  /// </summary>
  public sealed class ZhabaSessionFilter : SessionFilter
  {
    #region CONSTS
      public const string LOGIN_TOKEN_COOKIE_NAME = "Zhaba.loginToken";
      public const int    LOGIN_TOKEN_TIMEOUT_DAYS = 14;

      public static readonly byte[] LOGIN_TOKEN_ENCRYPTION_KEY = new byte[] //32 byte * 8 bit = 256 bit key
      {
        0x12, 0xDA, 0xCE, 0x50, 0xFE, 0x05, 0x78, 0x42, 0xA4, 0xAB, 0x9E, 0x23, 0x45, 0xDC, 0x3F, 0xA1,
        0x42, 0x37, 0xE3, 0x8B, 0x11, 0x01, 0xFF, 0x23, 0x15, 0xB1, 0xB3, 0x07, 0x9C, 0x00, 0x82, 0x44
      };

      public static readonly byte[] LOGIN_TOKEN_ENCRYPTION_IV = new byte[] //16 bytes * 8 bit = 128 bit vector
      {
        0xA2, 0x12, 0x01, 0x23, 0x12, 0x00, 0xE3, 0x39, 0x78, 0xAA, 0x19, 0x1E, 0x9D, 0xB6, 0xEE, 0x81
      };

    #endregion

    #region .ctor

      public ZhabaSessionFilter(WorkDispatcher dispatcher, string name, int order) : base(dispatcher, name, order) { }
      public ZhabaSessionFilter(WorkDispatcher dispatcher, IConfigSectionNode confNode) : base(dispatcher, confNode) { ctor(confNode); }
      public ZhabaSessionFilter(WorkHandler handler, string name, int order) : base(handler, name, order) { }
      public ZhabaSessionFilter(WorkHandler handler, IConfigSectionNode confNode) : base(handler, confNode) { ctor(confNode); }

      private void ctor(IConfigSectionNode confNode)
      {
        ConfigAttribute.Apply(this, confNode);
      }

    #endregion

    #region Protected

      protected override WaveSession MakeNewSessionInstance(WorkContext work)
      {
        var user = getValidatedUser(work); //null or Invalid user if could not be extracted from login token
        return new ZhabaWebSession(Guid.NewGuid(), user);
      }

      protected override WaveSession TryMakeSessionFromExistingLongTermToken(WorkContext work)
      {
        var user = getValidatedUser(work); //null or invalid user if could not be extracted from login token;
        if (user!=null && user.IsAuthenticated)
          return new ZhabaWebSession(Guid.NewGuid(), user);
        else
          return null;
      }

      protected override void StowSession(WorkContext work)
      {
        var session = work.Session as ZhabaWebSession;
        if (session!=null)
        {
          if (session.IsEnded) //DELETES long-term cookie
          {
            work.Response.SetClientVar(LOGIN_TOKEN_COOKIE_NAME, null);
          }
          else if (session.IsJustLoggedIn &&
                   session.ZhabaUser.IsAuthenticated &&
                   session.LastLoginType == NFX.ApplicationModel.SessionLoginType.Human &&
                   session.ZhabaUser.Status < UserStatus.Administrator) //DO NOT give long-term tokens to admins and above
          {
            work.Response.SetClientVar(LOGIN_TOKEN_COOKIE_NAME, encrypt(getToken(session)));
          }
        }

        base.StowSession(work);
      }


    #endregion

    #region .pvt

      private User getValidatedUser(WorkContext work)
      {
        var cv = work.Response.GetClientVar(LOGIN_TOKEN_COOKIE_NAME);

        if (cv.IsNotNullOrWhiteSpace())
        {
          var json = decrypt(cv);
          if (json==null) return null;

          var id = json["i"].AsULong();
          var issued = json["d"].AsDateTime();
          if ((App.TimeSource.UTCNow - issued).TotalDays > LOGIN_TOKEN_TIMEOUT_DAYS) return null;

          var atoken = new AuthenticationToken(Consts.ZHABA_SECURITY_REALM, id);
          return App.SecurityManager.Authenticate(atoken);
        }

        return null;
      }


      private JSONDataMap decrypt(string base64Data)
      {
        try
        {
          var edata = Convert.FromBase64String(base64Data);

          using(var rm = new RijndaelManaged(){Padding = PaddingMode.Zeros})
          using(var dec = rm.CreateDecryptor(LOGIN_TOKEN_ENCRYPTION_KEY, LOGIN_TOKEN_ENCRYPTION_IV))
          using(var msIn = new MemoryStream(edata))
          using(var cs = new CryptoStream(msIn, dec, CryptoStreamMode.Read))
          {
            var r = new StreamReader(cs);
            var txt = r.ReadToEnd();
            return JSONReader.DeserializeDataObject(txt) as JSONDataMap; //must be a valid JSON object
          }
        }
        catch
        {
          return null;
        }
      }

      private string encrypt(object token)
      {
        using(var rm = new RijndaelManaged(){Padding = PaddingMode.Zeros})
        using(var enc = rm.CreateEncryptor(LOGIN_TOKEN_ENCRYPTION_KEY, LOGIN_TOKEN_ENCRYPTION_IV))
        using(var msOut = new MemoryStream())
        {
          int len = 0;
          using(var cs = new CryptoStream(msOut, enc, CryptoStreamMode.Write))
          {
            JSONWriter.Write(token, new NFX.IO.NonClosingStreamWrap(cs), JSONWritingOptions.CompactASCII);
            if (!cs.HasFlushedFinalBlock) cs.FlushFinalBlock();
            len = (int)msOut.Length;
          }

          return Convert.ToBase64String(msOut.GetBuffer(), 0, len);
        }
      }

      private object getToken(ZhabaWebSession session)
      {
        var user = session.ZhabaUser;
        if (user.IsAuthenticated)
        {
          return new
                 {
                   r = NFX.ExternalRandomGenerator.Instance.NextScaledRandomInteger(0,100), //random noise
                   i = user.AuthToken.Data,                                                 //id
                   d = App.TimeSource.UTCNow,                                               //date
                 };
        }

        return new {};
      }

    #endregion

  }
}

