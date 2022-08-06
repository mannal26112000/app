using Estorenew.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Estorenew.Controllers
{
    public class ItemController : Controller
    {
        private ApplicationDbContext _context;
        public ItemController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                var items_cust = _context.Items.ToList();
                return View("AdminList",items_cust);
            }
            IList<Items> itemslist = new List<Items>();
            var username = HttpContext.User.Identity;
            var usernameid = User.Identity.GetUserId();
            var cart = _context.Cart.Include(m => m.ApplicationUser).Include(x=>x.items).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
            if (cart == null&& usernameid!=null)
            {
                _context.Cart.Add(new Cart { ApplicationUserId = usernameid, items = itemslist });
                _context.SaveChanges();
                cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
            }
            if (Session["GuestCart"] != null && cart.ApplicationUserId!=null)
            {
                cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                
                cart.items = itemslist;
                //var cart_items = Session["GuestCart"] as CartItem;
                //var all_users = _context.Users.ToList();
                if (Session["GuestCart"] != null)
                {
                    cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                    List<CartItem> previous_cart = JsonConvert.DeserializeObject<List<CartItem>>(Session["GuestCart"].ToString());

                    foreach (var obj in previous_cart)
                    {
                        //cart.items.Add(obj.Item);
                        //_context.Entry(obj.Item).State = EntityState.Unchanged;
                        _context.CartItem.Add(new CartItem() {ItemId=obj.Item.Id,CartId= cart.Id });

                    }
                    _context.SaveChanges();
                    Session.Clear();
                    //Session["GuestCart"].clear();
                    cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                }

            }
            var items = _context.Items.ToList();
            return View(items);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Items item)
        {
            if (item.Id == 0)
                _context.Items.Add(item);
            else
            {
                var itemInDb = _context.Items.Single(c => c.Id == item.Id);


                itemInDb.Name = item.Name;
                itemInDb.Price = item.Price;
                itemInDb.ItemInStock = item.ItemInStock;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Item");
        }
        [Authorize]
        public ActionResult NewItem()
        {
            return View();
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var items = _context.Items.SingleOrDefault(c => c.Id == id);
            if (items == null)
                return HttpNotFound();
            return View("NewItem", items);
        }
        [AllowAnonymous]
        public ActionResult AddItemToCart(int id)
        {
            var item = _context.Items.SingleOrDefault(c => c.Id == id);
            if (item == null)
                return HttpNotFound();
            return View(item);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveInCart(Items item)
        {
            IList<Items> itemslist = new List<Items>();
            var username = HttpContext.User.Identity;
            var usernameid = User.Identity.GetUserId();

            if (usernameid == null)
            {
                var cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                if (usernameid == null || cart == null)
                {
                    var ct = new Cart { items = itemslist };
                    //var guest_cart =JsonConvert.SerializeObject(new CartItem() { Cart = ct, Item = item });

                    //Session["GuestCart"] = guest_cart;
                    //Session["GuestCart"] = "data";
                    if (Session["GuestCart"] == null)
                    {
                        List<CartItem> list_cart = new List<CartItem> {
                            new CartItem() { Cart = ct, Item = item }
                        };
                        var guest_cart = JsonConvert.SerializeObject(list_cart);
                        Session["GuestCart"] = guest_cart;
                    }
                    else
                    {

                        List<CartItem> previous_cart = JsonConvert.DeserializeObject<List<CartItem>>(Session["GuestCart"].ToString());

                        previous_cart.Add(new CartItem() { Cart = ct, Item = item });
                        var guest_cart = JsonConvert.SerializeObject(previous_cart);
                        Session["GuestCart"] = guest_cart;
                    }
                }
            }
            else
            {
                var cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                if (cart == null)
                {
                    _context.Cart.Add(new Cart { ApplicationUserId = usernameid, items = itemslist });
                    _context.SaveChanges();
                    cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
                    cart.items = itemslist;
                }
                _context.CartItem.Add(new CartItem() { CartId = cart.Id, ItemId = item.Id });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult ViewCart()
        {
            var username = HttpContext.User.Identity;
            var usernameid = User.Identity.GetUserId();

            //List<CartItem> cart_items;
            if (usernameid == null)
            {
                if (Session["GuestCart"] != null)
                {
                    List<CartItem> previous_cart = JsonConvert.DeserializeObject<List<CartItem>>(Session["GuestCart"].ToString());

                    //cart_items = Session["GuestCart"] as List<CartItem>;
                    return View(previous_cart);
                }
                else
                {
                    return View(new List<CartItem>());
                }
            }
            else
            {
                //if (Session["GuestCart"] != null)
                //{
                //    List<CartItem> previous_cart = JsonConvert.DeserializeObject<List<CartItem>>(Session["GuestCart"].ToString());

                //    //cart_items = Session["GuestCart"] as List<CartItem>;
                //    return View(previous_cart);
                //}
                var cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();

                var current_cart_items = _context.CartItem.Where(m => m.CartId == cart.Id).ToList();//current cart

                var all_items = _context.Items.ToList();
                var temp = current_cart_items.Select(x => x);
                var temp2 = all_items.Select(x => x);
                return View(current_cart_items);
            }
        }
        [Authorize]
        public ActionResult Order()
        {
            var username = HttpContext.User.Identity;
            var usernameid = User.Identity.GetUserId();
            var cart = _context.Cart.Include(m => m.ApplicationUser).Where(m => m.ApplicationUserId == usernameid).FirstOrDefault();
            var reg_users = _context.Users.ToList();
            var all_carts = _context.Cart.Include(x => x.ApplicationUser).ToList();
            if (cart.ApplicationUser == null)
            {
                //var cart_items = Session["GuestCart"] as CartItem;
                //var all_users = _context.Users.ToList();
            }
            var current_cart_items = _context.CartItem.Where(m => m.CartId == cart.Id).ToList();//current cart
            foreach (var obj in current_cart_items)
            {
                _context.CartItem.Remove(obj);
            }
            _context.SaveChanges();
            return View();
        }
    }
}