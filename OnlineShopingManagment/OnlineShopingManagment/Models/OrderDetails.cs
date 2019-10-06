using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderID { get; set; }


        public int Quentity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Order Order { get; set; }
    }
}