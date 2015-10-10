using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ImageSharingWithModel.Controllers
{
    public class HomeController : BaseController
    {

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

        public ActionResult Error(String errid = "Unspecified")
        {
            CheckAda();
            if ("Details".Equals(errid))
            {
                ViewBag.Message = "Problem with Details action";
            }
            else if("EditNotAuth".Equals(errid))
            {
                ViewBag.Message = "Problem with EditNotAuth";
            }
            else
            {
                ViewBag.Meassage = "Error Unspecified";
            }
            return View();

        }

    }
}