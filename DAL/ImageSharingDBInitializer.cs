using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using ImageSharingWithModel.Models;

namespace ImageSharingWithModel.DAL
{
    public class ImageSharingDBInitializer : DropCreateDatabaseAlways<ImageSharingDB>
    {
        protected override void Seed(ImageSharingDB db)
        {
            db.Users.Add(new User { Userid = "Jason", ADA = false });
            db.Users.Add(new User { Userid = "Jay", ADA = false });

            db.Tags.Add(new Tag { Name = "portrait" });
            db.Tags.Add(new Tag { Name = "architecture" });

            db.SaveChanges();

            db.Images.Add(new Image
            {
                Caption = "head_photo",
                Description = "This is my head_photo",
                DateTaken = new DateTime(1993, 11, 9),
                UserId = 1,
                TagId = 1
            });
            db.SaveChanges();


            base.Seed(db); 
        }
    }
}