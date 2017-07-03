using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;

namespace Zhaba.Data.Filters
{
  public static class FilterParsers
  {
    /// <summary>
    /// Intelligently parses string respecting ID and email i.e:
    /// @yahoo.com dima@*.net jake -> screenname[0]=jake% email[0] -> %@yahoo.com email[1]dima@%.net
    /// The filters are to be connected with OR
    /// </summary>
    public static bool ParseIDFilter(this string idFilter, out List<string> screenNames, out List<string> emails, string wc = "%")
    {
      screenNames = null;
      emails = null;
      if (idFilter.IsNullOrWhiteSpace()) return false;
      var segs = idFilter.Split(' ', ',', ';').Where(s => s.IsNotNullOrWhiteSpace()).ToArray();

      if (segs.Length == 0) return false;

      foreach (var s in segs)
      {
        var iat = s.IndexOf('@');
        if (iat >= 0)//email
        {
          var email = iat == 0 ? '*' + s : s;
          if (iat == s.Length - 1) email = email + '*';

          email = email.Replace("*", wc);
          if (emails == null || !emails.Any(e => e.EqualsIgnoreCase(email)))
          {
            if (emails == null) emails = new List<string>();
            emails.Add(email);
          }
          continue;
        }

        //screen name
        var screenName = s;
        if (screenName.IndexOf('*') < 0)
          screenName = screenName + '*';

        screenName = screenName.Replace("*", wc);
        if (screenNames == null || !screenNames.Any(e => e.EqualsIgnoreCase(screenName)))
        {
          if (screenNames == null) screenNames = new List<string>();
          screenNames.Add(screenName);
        }
      }
      return screenNames != null || emails != null;
    }

    /// <summary>
    /// Intelligently parses a list of open/closed date spans, i.e.:
    ///  03/02/1987-,01/12/2012-02/14/2013
    /// </summary>
    public static bool ParseDateSpan(this string dateSpanFilter, out List<Tuple<DateTime?, DateTime?>> dates)
    {
      dates = null;
      if (dateSpanFilter.IsNullOrWhiteSpace()) return false;
      var segs = dateSpanFilter.Split(',', ';').Where(s => s.IsNotNullOrWhiteSpace()).ToArray();

      if (segs.Length == 0) return false;

      foreach (var s in segs)
      {
        DateTime? sd = null;
        DateTime? ed = null;

        var ih = s.IndexOf('-');
        if (ih < 0)
        {
          sd = s.AsNullableDateTime();
        }
        else
        {
          var from = ih > 0 ? s.Substring(0, ih) : string.Empty;
          var to = ih < s.Length - 1 ? s.Substring(ih + 1) : string.Empty;

          sd = from.AsNullableDateTime();
          ed = to.AsNullableDateTime();
        }

        if (sd.HasValue || ed.HasValue)
        {
          if (dates == null) dates = new List<Tuple<DateTime?, DateTime?>>();
          dates.Add(new Tuple<DateTime?, DateTime?>(sd, ed));
        }
      }
      return dates != null;
    }

    /// <summary>
    /// Intelligently parses a list of open/closed date spans, i.e.:
    ///  03/02/1987-,01/12/2012-02/14/2013
    /// </summary>
    public static bool ParseDate(this string dateSpanFilter, out List<Tuple<DateTime?, DateTime?>> dates)
    {
      dates = null;
      if (dateSpanFilter.IsNullOrWhiteSpace()) return false;
      var segs = dateSpanFilter.Split(',', ';').Where(s => s.IsNotNullOrWhiteSpace()).ToArray();

      if (segs.Length == 0) return false;

      foreach (var s in segs)
      {
        DateTime? sd = null;
        DateTime? ed = null;

        var ih = s.IndexOf('-');
        if (ih < 0)
        {
          sd = s.AsNullableDateTime();
        }
        else
        {
          var from = ih > 0 ? s.Substring(0, ih) : string.Empty;
          var to = ih < s.Length - 1 ? s.Substring(ih + 1) : string.Empty;

          sd = from.AsNullableDateTime();
          ed = to.AsNullableDateTime();
        }

        if (sd.HasValue || ed.HasValue)
        {
          if (dates == null) dates = new List<Tuple<DateTime?, DateTime?>>();
          var sdDate = sd.HasValue ? sd.Value.Date : (DateTime?) null;
          var edDate = ed.HasValue ? ed.Value.Date : (DateTime?) null;
          dates.Add(new Tuple<DateTime?, DateTime?>(sdDate, edDate));
        }
      }
      return dates != null;
    }


    public static string ParseName(this string nameFilter, string wc = "%")
    {
      if (nameFilter.IsNullOrWhiteSpace()) return null;
      nameFilter = nameFilter.Trim();
      if (nameFilter.Length == 0) return null;

      if (nameFilter.IndexOf('*') < 0) nameFilter += '*';

      nameFilter = nameFilter.Replace("*", wc);

      return nameFilter;
    }

    public static bool ParseInt(this string intFilter, out int result)
    {
      return int.TryParse(intFilter, out result);
    }
  }
}
