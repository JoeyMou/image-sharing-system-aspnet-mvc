using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithModel.Controllers
{
    public class BaseController : Controller
    {
        protected void CheckAda()
        {
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null)
            {
                if ("true".Equals(cookie["ADA"]))
                    ViewBag.isADA = true;
                else
                    ViewBag.isADA = false;
            }
            else
                ViewBag.isADA = false;
        }
        protected String GetLoggedInUser()
        {
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null && cookie["Userid"] != null)
            {
                return cookie["Userid"];
            }
            else
            {
                return null;
            }
        }
    }
}