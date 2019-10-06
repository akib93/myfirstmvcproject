using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public int quentity { get; set; }
        public float bill { get; set; }
        public int UnitPrice { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int InvoiceID { get; set; }
        public virtual Invoice Invoice { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
        
    }
}