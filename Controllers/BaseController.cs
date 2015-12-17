using ImageSharingWithCloudServices.DAL;
using ImageSharingWithCloudServices.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithCloudServices.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        protected BaseController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

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
        protected void SaveCookie(String userid, bool ADA)
        {
            // save in the cookie
            HttpCookie cookie = new HttpCookie("ImageSharing");
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.HttpOnly = true;
            cookie["ADA"] = ADA ? "true" : "false";
            Response.Cookies.Add(cookie);
        }
        protected IEnumerable<Image> ApprovedImages(IEnumerable<Image> images)
        {
            return images.Where(img => img.Approved);
        }
        protected IEnumerable<Image> ApprovedImages()
        {
            var db = new ApplicationDbContext();
            return ApprovedImages(db.Images);
        }


        protected ApplicationUser GetLoggedInUser()
        {
            return UserManager.FindById(User.Identity.GetUserId());
        }
    }
}