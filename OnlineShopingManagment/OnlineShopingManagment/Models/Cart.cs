using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.Models
{
    public class Cart
    {
        [Key]
        public int ProductId { get; set; }
        public string Productname { get; set; }
        public string Firstname { get; set; }
        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }
        public int Count { get; internal set; }
        public DateTime DateCreated { get; internal set; }
        public int RecordId { get; internal set; }
        public int Quentity { get; internal set; }

        internal static object GetCart(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        internal string GetCartId(HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}