using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;

using System.IO;
using System.Web.Script.Serialization;

using ImageSharingWithModel.Models;
using ImageSharingWithModel.DAL;
using System.Data.Entity;

namespace ImageSharingWithModel.Controllers
{
    public class ImagesController : BaseController
    {
        private ImageSharingDB db = new ImageSharingDB();

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
            ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View();
        }
        [HttpPost]
        public ActionResult Upload(ImageView image,
                                   HttpPostedFileBase ImageFile)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies.Get("ImageSharing");
                if (cookie != null)
                {
                    image.Userid = cookie["Userid"];
                    User User = db.Users.SingleOrDefault(u => u.Userid.Equals(image.Userid));

                    /* 
                     * Save image infomation in the database
                     */
                    Image imageEntity = new Image();
                    imageEntity.Caption = image.Caption;
                    imageEntity.Description = image.Description;
                    imageEntity.DateTaken = image.DateTaken;
                    imageEntity.UserId = User.Id;
                    imageEntity.TagId = image.TagId;
                    
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        db.Images.Add(imageEntity);
                        db.SaveChanges();
                        String imgFileName = Server.MapPath("~/Content/Images/img-" + imageEntity.Id + ".jpg");
                        ImageFile.SaveAs(imgFileName);

                        return RedirectToAction("Details", new {Id = imageEntity.Id } );
                    }
                    else
                    {
                        ViewBag.Message = "No image file specified!";
                        return View();
                    }
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
        public ActionResult Details(int Id)
        {
            CheckAda();            
            if (CheckIsRegistered() == true)
            {
                Image imageEnity = db.Images.Find(Id);
                if (imageEnity != null)
                {
                    return View(imageEnity);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "Details" });
                }
            }
            else
            {
                ViewBag.Message = "Please register before Querying!";
                return View("Query");
            }

        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            CheckAda();
            Image imageEntity = db.Images.Find(Id);
            if (CheckIsRegistered() == true)
            {
                if (imageEntity != null)
                {
                    ViewBag.Message = "";
                    ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", imageEntity.TagId);
                    ImageView image = new ImageView();
                    image.id = imageEntity.Id;
                    image.TagId = imageEntity.TagId;
                    image.Caption = imageEntity.Caption;
                    image.Description = imageEntity.Description;
                    image.DateTaken = imageEntity.DateTaken;

                    return View("Edit", imageEntity);
                }
                else
                {
                    ViewBag.Message = "Image with identifier " + Id + " not found";
                    ViewBag.Id = Id;
                    return RedirectToAction("Error", "Home", new { errid = "Edit" });
                }
            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
            }
            
        }
        [HttpPost]
        public ActionResult Edit(ImageView image)
        {
            CheckAda();
            if (ModelState.IsValid)
            {
                if (CheckIsRegistered() == true)
                {
                    Image imageEntity = db.Images.Find(image.id);
                    if (imageEntity != null)
                    {
                        HttpCookie cookie = Request.Cookies.Get("ImageSharing");
                        if (imageEntity.User.Userid.Equals(cookie["Userid"]))
                        {
                            imageEntity.Id = image.id;
                            imageEntity.TagId = image.TagId;
                            imageEntity.Caption = image.Caption;
                            imageEntity.Description = image.Description;
                            imageEntity.DateTaken = image.DateTaken;
                            db.Entry(imageEntity).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Details", new { Id = image.id });
                        }
                        else
                        {
                            return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                        }

                    }
                    else
                    {
                        return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                }

            }
            else
            {
                return View("Edit", image);
            }          
        }
        [HttpPost]
        public ActionResult Delete(ImageView image)
        {
            CheckAda();
            Image imageEntity = db.Images.Find(image.id);
            if (imageEntity != null)
            {
                db.Images.Remove(imageEntity);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "DeleteNotFound" });
            }

        }
        [HttpGet]
        public ActionResult ListAll(ImageView image)
        {
            CheckAda();
            IEnumerable<Image> images = db.Images.ToList();
            String userid = GetLoggedInUser();
            if (userid != null)
            {
                ViewBag.Userid = userid;
                return View(images);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpGet]
        public ActionResult ListByUser()
        {
            CheckAda();
            SelectList users = new SelectList(db.Users, "Id", "Userid", 1);
            return View(users);
        }
        [HttpGet]
        public ActionResult DoListByUser(int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser();
            if (userid != null)
            {
                User user = db.Users.Find(Id);
                if (user != null)
                {
                    ViewBag.Userid = userid;
                    return View("ListAll", user.Images);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "ListByUser" });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            SelectList tags = new SelectList(db.Tags, "Id", "Name   ", 1);
            return View(tags);
        }
        [HttpGet]
        public ActionResult DoListByTag(int Id)
        {
            CheckAda();
            String userid = GetLoggedInUser();
            if (userid != null)
            {
                Tag tag = db.Tags.Find(Id);
                if (tag != null)
                {
                    ViewBag.Userid = userid;
                    return View("ListAll", tag.Images);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "ListByTag" });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
    }
}