using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
namespace Final.Auth
{
    public class CustomAuth: ActionFilterAttribute, IAuthenticationFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id = (string)filterContext.HttpContext.Session["Role"];
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserName"]==null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
 
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {   
            if(filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new ViewResult { 
                    ViewName="Error"
                };

            }
        }
    }
}