using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

using ImageSharingWithCloudServices.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ImageSharingWithCloudServices.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }


    //public class ImageSharingDB : DbContext
    //{
    //    public DbSet<Image> Images { get; set; }
    //    public DbSet<User> Users { get; set; }
    //    public DbSet<Tag> Tags { get; set; }
    //}
}