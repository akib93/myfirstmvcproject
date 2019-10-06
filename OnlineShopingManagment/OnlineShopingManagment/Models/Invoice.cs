using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }
        public DateTime DateTime { get; set; }
        public float TotalBill { get; set; }
        public int ViewerID { get; set; }
        public virtual Viewer Viewer { get; set; }
       

        public virtual ICollection<Order> Orders { get; set; }
        public object OrderID { get; internal set; }
        public object Order { get; internal set; }
    }
}