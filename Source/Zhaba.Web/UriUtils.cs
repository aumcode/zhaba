using NFX;

namespace Zhaba.Web
{
  public static class URIS
  {
    public const string PROJECT_ID_PARAM  = "projID";
    public const string ISSUE_ID_PARAM    = "issueID";

    public const string HOME         = "/";
    public const string USER         = "/user";
    public const string USER_LOGIN   = USER + "/login";

    public const string COMMON            = "/common";
    public const string COMMON_USERS      = COMMON + "/users";
    public const string COMMON_USER       = COMMON + "/user";
    public const string COMMON_PROJECTS   = COMMON + "/projects";
    public const string COMMON_PROJECT    = COMMON + "/project";
    public const string COMMON_CATEGORIES = COMMON + "/categories";
    public const string COMMON_CATEGORY   = COMMON + "/category";

    public const string DASHBOARD          = "/dashboard";
    public const string DASHBOARD_PROJECTS = DASHBOARD + "/projects";

    public const string PROJECT            = "/project/{0}";
    public const string PROJECT_SELECT     = "/project/{0}/select";
    public const string PROJECT_MILESTONES = "/project/{0}/milestones";
    public const string PROJECT_MILESTONE  = "/project/{0}/milestone";
    public const string PROJECT_AREAS      = "/project/{0}/areas";
    public const string PROJECT_AREA       = "/project/{0}/area";
    public const string PROJECT_COMPONENTS = "/project/{0}/components";
    public const string PROJECT_COMPONENT  = "/project/{0}/component";
    public const string PROJECT_ISSUES     = "/project/{0}/issues";
    public const string PROJECT_ISSUE      = "/project/{0}/issue";

    public const string PROJECT_ISSUE_ISSUEASSIGN = "/project/{0}/issue/{1}/issueassign";



    public static string ForCOMMON_USER(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_USER, id) : COMMON_USER;
    }
    
    public static string ForCOMMON_PROJECT(ulong? id = null)
    {
      return id.HasValue ? "{0}?id={1}".Args(COMMON_PROJECT, id) : COMMON_PROJECT;
    }

    public static string ForCOMMON_CATEGORY(ulong? id = null)
    {
        return id.HasValue ? "{0}?id={1}".Args(COMMON_CATEGORY, id) : COMMON_CATEGORY;
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

    public static string ForPROJECT_ISSUE_AREA(ulong projID, ulong issue)
    {
      return ("/project/{0}/issuearea?issue={1}").Args(projID, issue);
    }

    public static string ForPROJECT_ISSUE_COMPONENT(ulong projID, ulong issue)
    {
      return ("/project/{0}/issuecomponent?issue={1}").Args(projID, issue);
    }

    public static string ForPROJECT_COMPONENTS(ulong projCounter)
    {
      return PROJECT_COMPONENTS.Args(projCounter);
    }

    public static string ForPROJECT_COMPONENT(ulong projCounter, ulong? counter = null)
    {
      return counter.HasValue ?
             (PROJECT_COMPONENT + "?id={1}").Args(projCounter, counter) :
              PROJECT_COMPONENT.Args(projCounter);
    }

    public static string ForPROJECT_AREAS(ulong projCounter)
    {
      return PROJECT_AREAS.Args(projCounter);
    }

    public static string ForPROJECT_AREA(ulong projCounter, ulong? counter = null)
    {
      return counter.HasValue ?
             (PROJECT_AREA + "?id={1}").Args(projCounter, counter) :
              PROJECT_AREA.Args(projCounter);
    }
  }
}
