using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudServices.Models
{
    public class ValidationRequest
    {
        public int imageId { get; set; }
        public string UserId { get; set; }
    }
}