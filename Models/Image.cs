using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudServices.Models
{
    public class Image
        /*
         * Entity model for an image
         */
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        [MaxLength(40)]
        public virtual String Caption { get; set; }
        [MaxLength(200)]
        public virtual String Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime DateTaken { get; set; }

        public virtual bool Validated { get; set; }
        public virtual bool Approved { get; set; }

        //[ForeignKey("ApplicationUser")]
        [Required]
        public virtual String UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("Tag")]
        public virtual int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        public virtual string Uri { get; set; }

        public Image()
        {
            Validated = false;
            Approved = false;
        }
    }
}