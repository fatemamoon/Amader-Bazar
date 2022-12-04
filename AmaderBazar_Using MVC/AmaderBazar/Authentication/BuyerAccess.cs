using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmaderBazar.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class BuyerAccess:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var context = HttpContext.Current;
            var flag = base.AuthorizeCore(httpContext);

            if (flag)
            {
                if (context.Session["AccType"] == null)
                    return false;
                else
                {
                    var s = context.Session["AccType"].ToString();
                    if (s == "Buyer")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }
}