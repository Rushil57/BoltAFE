using System;
using System.Web;
using System.Web.Mvc;

namespace BoltAFE.Helpers
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var session = HttpContext.Current.Session;
                var absolutePath = filterContext.HttpContext.Request.Url.AbsolutePath.ToLower();
                if (session == null || string.IsNullOrEmpty(Convert.ToString(session["UserId"])) || string.IsNullOrEmpty(Convert.ToString(session["RoleId"])))
                {
                    session.Abandon();
                    filterContext.Result = new RedirectResult("~/Login");
                    return;
                }
                //else if (!string.IsNullOrEmpty(Convert.ToString(session["RoleId"])) && Convert.ToInt32(session["RoleId"]) == 0 && (absolutePath.StartsWith("/usermaster") || absolutePath.StartsWith("/userdefinition") || absolutePath.StartsWith("/admin")))
                //{
                //    filterContext.Result = new RedirectResult("~/Dashboard");
                //    return;
                //}
                else
                {
                    if (Convert.ToBoolean(session["ResetPassword"]) && !absolutePath.StartsWith("/resetpassword"))
                    {
                        filterContext.Result = new RedirectResult("~/ResetPassword");
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}