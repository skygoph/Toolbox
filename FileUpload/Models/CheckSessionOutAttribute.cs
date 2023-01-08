using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUpload.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllername = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            if (!controllername.Contains("home"))
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                var user = session["Username"];
                if (((user == null) && (!session.IsNewSession)) || (session.IsNewSession))
                {
                    var url = new UrlHelper(filterContext.RequestContext);
                    var loginUrl = url.Content("~/User/LogIn");
                    filterContext.HttpContext.Response.Redirect(loginUrl);
                }

            }
            base.OnActionExecuting(filterContext);
        }
    }
}