using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ImageSharingWithModel.DAL;
using ImageSharingWithModel.Models;

using System.Data.Entity;

namespace ImageSharingWithModel.Controllers
{
    public class AccountController : BaseController
    {

        private ImageSharingDB db = new ImageSharingDB();

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
        public ActionResult Register(UserView UserInfo)
        {
            CheckAda();

            if (ModelState.IsValid) {
                User User = db.Users.SingleOrDefault(u => u.Userid.Equals(UserInfo.Userid));
                if (User == null)
                {
                    // save to database
                    User = new User(UserInfo.Userid, UserInfo.ADA);
                    db.Users.Add(User);
                }
                else
                {
                    User.ADA = UserInfo.ADA;
                    db.Entry(User).State = EntityState.Modified;
                }
                db.SaveChanges();
                

                SaveCookie(UserInfo.Userid, UserInfo.ADA);
               
            }

            ViewBag.Userid = UserInfo.Userid; 
            return View("RegisterSuccess");
        }

        [HttpGet]
        public ActionResult Login()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }
        [HttpGet]
        public ActionResult DoLogin(String Userid)
        {
            CheckAda();
            User User = db.Users.SingleOrDefault(u => u.Userid.Equals(Userid));
            if (User != null)
            {
                SaveCookie(Userid, User.ADA);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "No such user registered!";
                return View("Login");
            }
        }

        protected void SaveCookie(String userid, bool ADA)
        {
            // save in the cookie
            HttpCookie cookie = new HttpCookie("ImageSharing");
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie["Userid"] = userid;
            cookie["ADA"] = ADA ? "true" : "false";
            Response.Cookies.Add(cookie);
        }
    }
}