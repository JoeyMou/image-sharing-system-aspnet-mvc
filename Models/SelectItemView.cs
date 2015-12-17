using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudServices.Models
{
    public class SelectItemView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public SelectItemView(string id, string name, bool c)
        {
            this.Id = id;
            this.Name = name;
            this.Checked = c;
        }
        public SelectItemView() { }
    }
}