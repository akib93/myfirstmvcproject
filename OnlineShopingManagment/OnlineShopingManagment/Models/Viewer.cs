using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.Models
{
    public class Viewer
    {
        [Key]
        public int ViewerID { get; set; }
        public string ViewerName { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}