using NFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaba.Web
{
  public static class URIS
  {
    public const string PROJECT_ID_PARAM = "projID";

    public const string HOME         = "/";
    public const string USER         = "/user";
    public const string USER_LOGIN   = USER + "/login";

    public const string COMMON            = "/common";
    public const string COMMON_USERS      = COMMON + "/users";
    public const string COMMON_USER       = COMMON + "/user";
    public const string COMMON_COMPONENTS = COMMON + "/components";
    public const string COMMON_COMPONENT  = COMMON + "/component";
    public const string COMMON_AREAS      = COMMON + "/areas";
    public const string COMMON_AREA       = COMMON + "/area";
    public const string COMMON_PROJECTS   = COMMON + "/projects";
    public const string COMMON_PROJECT    = COMMON + "/project";

    public const string DASHBOARD          = "/dashboard";
    public const string DASHBOARD_PROJECTS = DASHBOARD + "/projects";

    public const string PROJECT            = "/project/{0}";
    public const string PROJECT_SELECT     = "/project/{0}/select";
    public const string PROJECT_MILESTONES = "/project/{0}/milestones";
    public const string PROJECT_MILESTONE  = "/project/{0}/milestone";
    public const string PROJECT_AREAS      = "/project/{0}/areas";
    public const string PROJECT_ISSUES     = "/project/{0}/issues";
    public const string PROJECT_ISSUE      = "/project/{0}/issue";



    public static string ForCOMMON_USER(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_USER, id) : COMMON_USER;
    }

    public static string ForCOMMON_COMPONENT(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_COMPONENT, id) : COMMON_COMPONENT;
    }

    public static string ForCOMMON_AREA(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_AREA, id) : COMMON_AREA;
    }

    public static string ForCOMMON_PROJECT(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_PROJECT, id) : COMMON_PROJECT;
    }

    public static string ForPROJECT_SELECT(ulong projID)
    {
      return PROJECT_SELECT.Args(projID);
    }

    public static string ForPROJECT_MILESTONES(ulong projID)
    {
      return PROJECT_MILESTONES.Args(projID);
    }

    public static string ForPROJECT_MILESTONE(ulong projID, ulong? id = null)
    {
      return id.HasValue ?
             (PROJECT_MILESTONE+"?id={1}").Args(projID, id) :
              PROJECT_MILESTONE.Args(projID);
    }

    public static string ForPROJECT_ISSUES(ulong projID)
    {
      return PROJECT_ISSUES.Args(projID);
    }

    public static string ForPROJECT_ISSUE(ulong projID, ulong? id = null)
    {
      return id.HasValue ?
             (PROJECT_ISSUE+"?id={1}").Args(projID, id) :
              PROJECT_ISSUE.Args(projID);
    }
  }
}
