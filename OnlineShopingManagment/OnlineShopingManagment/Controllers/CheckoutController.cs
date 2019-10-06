using OnlineShopingManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopingManagment.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
     
        ApplicationDbContext db = new ApplicationDbContext();
        const string PromoCode = "50";
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);


            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                     StringComparison.OrdinalIgnoreCase
                    ) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    

                    db.Orders.Add(order);
                    db.SaveChanges();


                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete", new { id = order.OrderID });
                }
            }
            catch
            {
                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.Orders.Any(o => o.OrderID == id &&
                           o.Username == User.Identity.Name
            );
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}