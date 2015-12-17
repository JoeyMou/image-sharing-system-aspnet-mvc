using ImageSharingWithCloudServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ImageSharingWithCloudServices.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            CheckAda();
            ApplicationUser user = GetLoggedInUser();
            if (user != null)
                ViewBag.Userid = user.UserName;
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