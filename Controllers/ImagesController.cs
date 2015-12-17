using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;

using System.IO;
using System.Web.Script.Serialization;

using ImageSharingWithCloudServices.Models;
using ImageSharingWithCloudServices.DAL;
using System.Data.Entity;

namespace ImageSharingWithCloudServices.Controllers
{
    [Authorize]
    public class ImagesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ValidationQueue validationQ = new ValidationQueue();

        protected Boolean CheckIsRegistered()
        {
            ApplicationUser user = GetLoggedInUser();
            if (user != null)
                return true;
            else
                return false;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            CheckAda();
            CheckIsRegistered();
            ViewBag.Message = "";
            ViewData["tags"] = new SelectList(db.Tags, "Id", "Name", 1);
            return View();
        }
        [HttpPost]
        public ActionResult Upload(ImageView image,
                                   HttpPostedFileBase ImageFile)
        {
            CheckAda();
            ApplicationUser user = GetLoggedInUser();
            ViewData["tags"] = new SelectList(db.Tags, "Id", "Name", 1);
            if (ModelState.IsValid)
            {
                /* 
                    * Save image infomation in the database
                    */
                Image imageEntity = new Image();
                imageEntity.Caption = image.Caption;
                imageEntity.Description = image.Description;
                imageEntity.DateTaken = image.DateTaken;
                imageEntity.TagId = image.TagId;
                imageEntity.UserId = user.Id;
                imageEntity.Validated = false;
                imageEntity.Approved = false;
                    
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    if (!ImageFile.ContentType.Equals("image/jpeg"))
                    {
                        ViewBag.Message = "The file you uploaded is not a jpg file";
                        return View();
                    }
                    else
                    {
                        db.Images.Add(imageEntity);
                        db.SaveChanges();
                        db.Images.Find(imageEntity.Id).Uri = ImageStorage.ImageURI(Url, imageEntity.Id);
                        db.SaveChanges();
                        ImageStorage.SaveFile(Server, ImageFile, imageEntity.Id);
                        // string imgFileName = Server.MapPath("~/Content/Images/img-" + imageEntity.Id + ".jpg");
                        // ImageFile.SaveAs(imgFileName);

                        ValidationRequest validationReq = new ValidationRequest();
                        validationReq.imageId = imageEntity.Id;
                        validationReq.UserId = user.Id;

                        return RedirectToAction("Details", new { Id = imageEntity.Id });
                    }
                        
                }
                else
                {
                    ViewBag.Message = "No image file specified!";
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
            CheckIsRegistered();
            ViewBag.Message = "";
            return View();
        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            CheckAda();
            Image imageEnity = db.Images.Find(Id);
            if (imageEnity != null)
            {
                LogContext.AddLogEntry(GetLoggedInUser(), imageEnity);
                return View(imageEnity);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { errid = "Details" });
            }          
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            CheckAda();
            Image imageEntity = db.Images.Find(Id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageView image)
        {
            CheckAda();
            if (ModelState.IsValid)
            {
                ApplicationUser user = GetLoggedInUser();
                Image imageEntity = db.Images.Find(image.id);
                if (imageEntity != null)
                {                       
                    if (imageEntity.UserId.Equals(user.Id))
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
                    return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                }

            }
            else
            {
                return View("Edit", image);
            }          
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            CheckAda();
            Image imageEntity = db.Images.Find(id);
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
            IEnumerable<Image> images = ApprovedImages().ToList();
            ApplicationUser user = GetLoggedInUser();
            if (user != null)
            {
                ViewBag.Userid = user.Id;
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
            SelectList users = new SelectList(db.Users, "Id", "UserName", 1);
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoListByUser(string Id)
        {
            CheckAda();
            ViewBag.Userid = GetLoggedInUser().Id;
            ApplicationUser user = db.Users.Find(Id);
            return View("ListAll", ApprovedImages(user.Images).ToList());
        }
        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            SelectList tags = new SelectList(db.Tags, "Id", "Name   ", 1);
            return View(tags);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoListByTag(int Id)
        {
            CheckAda();
            ApplicationUser user = GetLoggedInUser();
            if (user != null)
            {
                Tag tag = db.Tags.Find(Id);
                if (tag != null)
                {
                    ViewBag.Userid = user.Id;
                    return View("ListAll", ApprovedImages(tag.Images).ToList());
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


        [HttpGet]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve()
        {
            CheckAda();
            ViewBag.Message = "";
            var db = new ApplicationDbContext();
            List<SelectItemView> model = new List<SelectItemView>();
            foreach (var u in db.Images)
            {
                if (u.Validated && !u.Approved)
                {
                    model.Add(new SelectItemView(u.Id.ToString(), u.Caption, !u.Approved));
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve(List<SelectItemView> model)
        {
            CheckAda();
            var db = new ApplicationDbContext();
            foreach (var imod in model)
            {
                Image image = db.Images.Find(Int32.Parse(imod.Id));
                if (imod.Checked)
                {
                    image.Approved = true;
                }
                imod.Name = image.Caption;
                db.SaveChanges();
                ViewBag.Message = "Images successfully approved.";
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public ActionResult ImageViews(ImageView image)
        {
            CheckAda();
            IEnumerable<LogEntry> entries = LogContext.Select();
            return View(entries);
        }
    }
}