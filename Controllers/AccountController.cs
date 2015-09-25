using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithUpload.Controllers
{
    public class AccountController : Controller
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



        // GET: Account
        public ActionResult Index()
        {
            CheckAda();
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            CheckAda();
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null)
            {
                ViewBag.Userid = cookie["Userid"];
                if ("true".Equals(cookie["ADA"]))
                    ViewBag.ADA = true;
                else
                    ViewBag.ADA = false;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(String Userid, Boolean ADA)
        {
            CheckAda();
            HttpCookie cookie = new HttpCookie("ImageSharing");
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie["Userid"] = Userid;
            cookie["ADA"] = ADA ? "true" : "false";
            Response.Cookies.Add(cookie);
 

            ViewBag.Userid = Userid; 
            return View("RegisterSuccess");
        }
    }
}