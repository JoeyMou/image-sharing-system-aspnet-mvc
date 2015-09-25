using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ImageSharingWithUpload.Controllers
{
    public class HomeController : Controller
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



        public ActionResult Index()
        {
            CheckAda();
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null)
                ViewBag.Userid = cookie["Userid"];
            else
                ViewBag.Userid = "Stranger";

            return View();
        }

        

    }
}