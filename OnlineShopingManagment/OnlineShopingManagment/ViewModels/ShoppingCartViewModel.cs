using OnlineShopingManagment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopingManagment.ViewModels
{
    public class ShoppingCartViewModel
    {
        [Key]
        public int ShoppingCartViewModelID { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}