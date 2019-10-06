using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopingManagment.Models
{
    public class ShoppingCart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "ProductId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }



        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Product product)
        {
            var cartItem = db.Carts.SingleOrDefault(
                c => c.Productname == ShoppingCartId
                && c.ProductId == product.ProductID
                );

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.ProductID,
                    Productname = ShoppingCartId,
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
                c => c.Productname == ShoppingCartId
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
                cart => cart.Productname == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }


        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.Productname == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.Productname == ShoppingCartId
                          select (int?)cartItems.Count
                        ).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.Productname == ShoppingCartId
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
        public void MigrateCart(string username)

        {
            var ShoppingCart = db.Carts.Where(c => c.Productname == ShoppingCartId);

            foreach (Cart item in ShoppingCart)
            {
                item.Productname = username;
            }
            db.SaveChanges();
        }


    }
}