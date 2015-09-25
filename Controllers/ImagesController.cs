using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Web.Script.Serialization;

using ImageSharingWithUpload.Models;

namespace ImageSharingWithUpload.Controllers
{
    public class ImagesController : Controller
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

        protected Boolean CheckIsRegistered()
        {
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null)
                return true;
            else
                return false;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        public ActionResult Upload(Image image,
                                   HttpPostedFileBase ImageFile)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies.Get("ImageSharing");
                if (cookie != null)
                {
                    image.Userid = cookie["Userid"];
                    /*
                     * Save image information on the server file system   
                     */
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    String jsonData = serializer.Serialize(image);

                    String filename = Server.MapPath("~/App_Data/Image_Info/" + image.Id + ".js");

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        String imgFileName = Server.MapPath("~/Content/Images/" + image.Id + ".jpg");
                        ImageFile.SaveAs(imgFileName);
                        System.IO.File.WriteAllText(filename, jsonData);
                    }


                    return View("QuerySuccess", image);
                }
                else
                {
                    ViewBag.Message = "Please register before uploading!";
                    return View();
                }

            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form!";
                return View();
            }

        }


        [HttpGet]
        public ActionResult Query()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }
        [HttpGet]
        public ActionResult Details(String Id)
        {
            CheckAda();
            if (CheckIsRegistered() == true)
            {
                String filename = Server.MapPath("~/App_Data/Image_Info/" + Id + ".js");
                if (System.IO.File.Exists(filename))
                {
                    String jsonData = System.IO.File.ReadAllText(filename);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Image image = serializer.Deserialize<Image>(jsonData);
                    return View("QuerySuccess", image);
                }
                else
                {
                    ViewBag.Message = "Image with identifier " + Id + " not found";
                    ViewBag.Id = Id;
                    return View("Query");
                }
            }
            else
            {
                ViewBag.Message = "Please register before Querying!";
                return View("Query");
            }

        }
    }
}