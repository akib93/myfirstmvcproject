using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

using OnlineShopingManagment.Models;
namespace OnlineShopingManagment.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            Session["ViewerID"] = 1;
            if (TempData["Cart"] != null)
            {
                float x = 0;
                List<Cart> li2 = TempData["Cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;
                }
                TempData["total"] = x;
            }
            TempData.Keep();

            if (searchString != null)
            {
                page = 1;
            }
            else { searchString = currentFilter; }
            ViewBag.CurrentFilter = searchString;

            var product = from s in db.Products
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    product = product.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    product = product.OrderBy(s => s.ProductName);
                    break;
                case "01":
                    Session["ViewerID"] = 1;
                    if (TempData["Cart"] != null)
                    {
                        float x = 0;
                        List<Cart> li2 = TempData["Cart"] as List<Cart>;
                        foreach (var item in li2)
                        {
                            x += item.bill;
                        }
                        TempData["total"] = x;
                    }
                    TempData.Keep();
                    return View(db.Products.OrderByDescending(x => x.ProductID).ToList());
                    
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(product.ToPagedList(pageNumber, pageSize));       
        }

        public ActionResult AddtoCard(int? Id)
        {
            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();
            return View(p);
        }

        List<Cart> li = new List<Cart>();

        public string ShoppingCartId { get; private set; }

        [HttpPost]
        public ActionResult AddtoCard(Product pi, string qty, int Id)
        {
            Product p = db.Products.Where(x => x.ProductID == Id).SingleOrDefault();
            Cart c = new Cart();
            c.ProductId = p.ProductID;
            c.price = (float)p.Price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.Productname = p.ProductName;
            if (TempData["Cart"] == null)
            {
                li.Add(c);

                TempData["Cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["Cart"] as List<Cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.Productname == c.Productname)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                TempData["Cart"] = li2;
            }


            TempData.Keep();

            return RedirectToAction("Index");
        }

        public ActionResult remove(string name)
        {
            List<Cart> li2 = TempData["Cart"] as List<Cart>;
            Cart c = li2.Where(x => x.Productname == name).SingleOrDefault();
            li2.Remove(c);
            float h = 0;
            foreach (var item in li2)
            {
                h += item.bill;
            }
            TempData["total"] = h;
            return RedirectToAction("CheckOut");
        }




        public ActionResult CheckOut()
        {
            TempData.Keep();
            return View();
           
        }

        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            List<Cart> li = TempData["Cart"] as List<Cart>;

            Invoice iv = new Invoice();
            iv.ViewerID = Convert.ToInt32(Session["ViewerID"].ToString());
            iv.DateTime = System.DateTime.Now;
            iv.TotalBill = (float)TempData["total"];
            db.Invoices.Add(iv);
            db.SaveChanges();

            foreach (var item in li)
            {
                Order od = new Order();
                od.ProductID = item.ProductId;
                od.InvoiceID = iv.InvoiceID;
                od.Date = System.DateTime.Now;
                od.quentity = item.qty;
                od.Firstname = item.Firstname;
                od.UnitPrice = (int)item.price;
                od.bill = item.bill;
                db.Orders.Add(od);
                db.SaveChanges();
            }

            TempData.Remove("total");
            TempData.Remove("Cart");
            TempData["msg"] = "Transaction Completed......";
            TempData.Keep();
            return RedirectToAction("Index");
        }

        public void MigrateCart(string Email)

        {
            var ShoppingCart = db.Carts.Where(c => c.Productname == ShoppingCartId);

            foreach (Cart item in ShoppingCart)
            {
                item.Productname = Email;
            }
            db.SaveChanges();
        }

       
        const string PromoCode = "50";
        [Authorize]
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


                    var cart = Cart.GetCart(this.HttpContext);
                    

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






        string Productname { get; set; }
        public const string CartSessionKey = "Productname";
        public static Cart GetCart(HttpContextBase context)
        {
            var cart = new Cart();
            cart.Productname = cart.GetCartId(context);
            return cart;
        }



        public static Cart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Product product)
        {
            var cartItem = db.Carts.SingleOrDefault(
                c => c.Productname == Productname
                && c.ProductId == product.ProductID
                );

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.ProductID,
                    Productname = Productname,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            db.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(
                c => c.Productname == Productname
                && c.RecordId == id
                );

            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.Productname == Productname);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }


        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.Productname == Productname).ToList();
        }
        public int GetCount()
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.Productname == Productname
                          select (int?)cartItems.Count
                        ).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.Productname == Productname
                              select (int?)cartItems.Count
                            ).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {

                    OrderID = order.OrderID,
                    UnitPrice = item.
                    Quentity = item.Count

                };

                orderTotal += item.Count;
                db.OrderDetails.Add(orderDetail);
            }
            order.Total = orderTotal;
            db.SaveChanges();
            EmptyCart();
            return order.OrderID;
        }

        private string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;

                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }













    }
}