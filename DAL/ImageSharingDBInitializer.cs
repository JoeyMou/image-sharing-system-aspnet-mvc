using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using ImageSharingWithCloudServices.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;



namespace ImageSharingWithCloudServices.DAL
{
    public class ImageSharingDBInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext db)
        {
            //if (db.Database.Exists())
            //{
            //    // single-user mode not available in Azure SQL Database
            //    db.Database.Delete();
            //}
            //db.Database.Create();

            this.Seed(db);
        }

        protected void Seed(ApplicationDbContext db)
        {

            //RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            //UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);

            //RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(roleStore);
            //UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(userStore);

            //IdentityResult ir;

            //ApplicationUser nobody = createUser("nobody@example.org");
            //ApplicationUser jfk = createUser("jfk@example.org");
            //ApplicationUser nixon = createUser("nixon@example.org");
            //ApplicationUser fdr = createUser("fdr@example.org");


            //ir = um.Create(nobody, "nobody1234");
            //ir = um.Create(jfk, "jfk1234");
            //ir = um.Create(nixon, "nixon1234");
            //ir = um.Create(fdr, "fdr1234");

            //rm.Create(new IdentityRole("User"));
            //if (!um.IsInRole(nobody.Id, "User"))
            //{
            //    um.AddToRole(nobody.Id, "User");
            //}
            //if (!um.IsInRole(jfk.Id, "User"))
            //{
            //    um.AddToRole(jfk.Id, "User");
            //}
            //if (!um.IsInRole(nixon.Id, "User"))
            //{
            //    um.AddToRole(nixon.Id, "User");
            //}
            //if (!um.IsInRole(fdr.Id, "User"))
            //{
            //    um.AddToRole(fdr.Id, "User");
            //}

            //rm.Create(new IdentityRole("Admin"));
            //if (!um.IsInRole(nixon.Id, "Admin"))
            //{
            //    um.AddToRole(nixon.Id, "Admin");
            //}

            //rm.Create(new IdentityRole("Approver"));
            //if (!um.IsInRole(jfk.Id, "Approver"))
            //{
            //    um.AddToRole(nixon.Id, "Approver");
            //}

            //rm.Create(new IdentityRole("Supervisor"));
            //if (!um.IsInRole(fdr.Id, "Supervisor"))
            //{
            //    um.AddToRole(fdr.Id, "Supervisor");
            //}

            //db.Tags.Add(new Tag { Name = "portrait" });
            //db.Tags.Add(new Tag { Name = "architecture" });

            //db.SaveChanges();

            //db.Images.Add(new Image
            //{
            //    Caption = "head_photo",
            //    Description = "This is my head_photo",
            //    DateTaken = new DateTime(1993, 11, 9),
            //    UserId = jfk.Id,
            //    TagId = 1,
            //    Approved = true,
            //    Uri = "https://imagesharingmou.blob.core.windows.net/images/6.jpg"
            //});
            //db.SaveChanges();

            //LogContext.CreateTable();



            //base.Seed(db);
        }
        private ApplicationUser createUser(String userName)
        {
            return new ApplicationUser { UserName = userName, Email = userName };
        }
    }

}