using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithCloudServices.Models
{
    public class Tag
    {
        [Key]
        public virtual int Id { get; set; }
        [MaxLength(20)]
        public virtual String Name { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}