using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

using ImageSharingWithModel.Models;

namespace ImageSharingWithModel.DAL
{
    public class ImageSharingDB : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}