using AmaderBazar.Authentication;
using AmaderBazar.BusinessModel;
using AmaderBazar.Models;
using AmaderBazar.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AmaderBazar.Controllers
{
    [BuyerAccess]
    public class BuyerController : Controller
    {
        // GET: Buyer
        [HttpGet]
        public ActionResult Home()
        {
            if (Session["Filter"] == null)
                Session["Filter"] = "Name";
            var bind = new BuyerHomeBind();
            if (TempData["search"] == null)
            {
                var db = new AmaderBazarEntities();
                if (Session["Filter"].ToString() == "Name")
                    bind.Products = (from i in db.Products
                                     orderby i.PName
                                     select i).ToList();
                else if (Session["Filter"].ToString() == "Price")
                    bind.Products = (from i in db.Products
                                     orderby i.Price
                                     select i).ToList();
                bind.Categories = (from i in db.Categories
                                   orderby i.CatName
                                   select i).ToList();
                bind.TotalProducts = bind.Products.Count();
            }
            else
            {
                bind = TempData["search"] as BuyerHomeBind;
                TempData.Remove("search");
            }
            /*Session.Remove("searchType");
            Session.Remove("search");*/
            bind.Filter = Session["Filter"].ToString();
            return View(bind);
        }

        //[HttpPost]
        public ActionResult SearchByProduct(string search)
        {
            Session["searchType"] = "Product";
            Session["search"] = search;
            if (TempData["search"] != null)
                TempData.Remove("search");

            var bind = new BuyerHomeBind();
            if (search != "")
            {
                var db = new AmaderBazarEntities();

                if (Session["Filter"].ToString() == "Name")
                    bind.Products = (from i in db.Products
                                     orderby i.PName
                                     where i.PName.StartsWith(search)
                                     select i
                         ).ToList();
                else
                    bind.Products = (from i in db.Products
                                     orderby i.Price
                                     where i.PName.StartsWith(search)
                                     select i
                         ).ToList();
                bind.Categories = (from i in db.Categories
                                   orderby i.CatName
                                   select i).ToList();
                bind.Search = search;
                bind.SearchType = "Product";

                if (bind.Products.Count() == 0)
                    bind.SearchMsg = "No products found.";

                TempData["search"] = bind;
                bind.TotalProducts = (from p in db.Products
                                      select p).ToList().Count();
                return RedirectToAction("Home");
            }
            else
            {
                return RedirectToAction("Home");
            }

        }
        public ActionResult Refresh()
        {
            if (TempData["search"] != null)
                TempData.Remove("search");

            Session.Remove("searchType");
            Session.Remove("search");
            Session["Filter"] = "Name";

            return RedirectToAction("Home");
        }

        //[HttpGet]
        public ActionResult SearchByCategory(string search)
        {
            Session["searchType"] = "Category";
            Session["search"] = search;

            if (TempData["search"] != null)
                TempData.Remove("search");

            var bind = new BuyerHomeBind();
            if (search != "")
            {
                var db = new AmaderBazarEntities();

                if (Session["Filter"].ToString() == "Name")
                    bind.Products = (from i in db.Products
                                     orderby i.PName
                                     where i.CatName.Contains(search)
                                     select i).ToList();
                else
                    bind.Products = (from i in db.Products
                                     orderby i.Price
                                     where i.CatName.Contains(search)
                                     select i).ToList();

                bind.Categories = (from i in db.Categories
                                   orderby i.CatName
                                   select i).ToList();
                bind.Search = search;
                bind.SearchType = "Category";

                if (bind.Products.Count() == 0)
                    bind.SearchMsg = "No products found in this category.";

                TempData["search"] = bind;
                bind.TotalProducts = (from p in db.Products
                                      select p).ToList().Count();
                return RedirectToAction("Home");
            }
            else
            {
                return RedirectToAction("Home");
            }

        }

        public ActionResult Decide(string Filter)
        {
            Session["Filter"] = Filter;
            var a = Session["Filter"].ToString();
            if (Session["searchType"] == null)
            {
                return RedirectToAction("Home");
            }
            else if (Session["searchType"].ToString() == "Product")
            {
                return RedirectToAction("SearchByProduct", "Buyer", new { search = Session["search"].ToString() });
            }
            else
            {
                return RedirectToAction("SearchByCategory", "Buyer", new { search = Session["search"].ToString() });
            }
        }

        public ActionResult Cart()
        {
            if (Session["cart"] != null)
            {
                string j = (string)Session["cart"];
                var d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(j);
                return View(d);
            }
            else
            {
                return View();
            }
        }

        public ActionResult AddToCart(int id, string Qty)
        {
            var db = new AmaderBazarEntities();
            var p = (from i in db.Products
                     where i.PID == id
                     select new CartProduct { PID = i.PID, PName = i.PName, Qty = i.Qty, Price = i.Price }).FirstOrDefault();

            int quantity = 1;
            if (Qty != "")
                quantity = Convert.ToInt32(Qty);
            if (p.Qty != 0)
            {
                p.Qty = quantity;

                string json = "";
                if (Session["cart"] == null)
                {
                    var d = new List<CartProduct>();
                    d.Add(p);
                    json = new JavaScriptSerializer().Serialize(d);
                    Session["cart"] = json;
                }
                else
                {
                    json = Session["cart"].ToString();
                    var d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(json);
                    bool flag = false;
                    for (int i = 0; i < d.Count; i++)
                    {
                        if (d[i].PID == p.PID)
                        {
                            d[i].Qty = d[i].Qty + p.Qty;
                            d[i].Price = d[i].Qty * p.Price;
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        d.Add(p);
                    json = new JavaScriptSerializer().Serialize(d);
                    Session["cart"] = (object)json;
                }
            }

            if (Session["searchType"] != null)
            {

                if (Session["searchType"].ToString() == "Category")
                {
                    return RedirectToAction("SearchByCategory", "Buyer", new { search = Session["search"].ToString() });
                }
                else
                {

                    return RedirectToAction("SearchByProduct", "Buyer", new { search = Session["search"].ToString() });
                }
            }
            else
            {
                return RedirectToAction("Home");
            }


        }

        public ActionResult UpdateCart(int id, int Qty)
        {
            if (Session["cart"] != null)
            {
                string json = Session["cart"].ToString();
                var d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(json);
                for (int i = 0; i < d.Count; i++)
                {
                    if (d[i].PID == id && d[i].Qty + Qty > 0)
                    {
                        double temp = d[i].Price / d[i].Qty;
                        d[i].Qty = d[i].Qty + Qty;
                        d[i].Price = d[i].Qty * temp;
                        break;
                    }
                }
                json = new JavaScriptSerializer().Serialize(d);
                Session["cart"] = (object)json;
            }
            return RedirectToAction("Cart");
        }

        public ActionResult DeleteFromCart(int id)
        {
            string j = (string)Session["cart"];

            List<CartProduct> d = new List<CartProduct>();
            d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(j);

            var itemToRemove = d.Single(r => r.PID == id);
            d.Remove(itemToRemove);

            if (d.Count() != 0)
            {
                j = new JavaScriptSerializer().Serialize(d);
                Session["cart"] = (object)j;
            }
            else
                Session.Remove("cart");

            return RedirectToAction("Cart");
        }

        public ActionResult CancelOrder()
        {
            if (Session["cart"] != null)
            {
                Session.Remove("cart");
            }
            return RedirectToAction("Cart");
        }

        public ActionResult CancelTransaction(int id)
        {
            var db = new AmaderBazarEntities();

            var d = (from i in db.Transactions
                     where i.TID == id
                     select i).FirstOrDefault();

            d.Status = "Canceled";
            d.DMID = null;
            db.SaveChanges();

            var temp = new JavaScriptSerializer().Deserialize<List<CartProduct>>(d.TDetials);

            foreach(var i in temp)
            {
                var p = (from j in db.Products
                         where j.PID == i.PID
                         select j).FirstOrDefault();

                p.Qty += i.Qty;
                db.SaveChanges();
            }

            return RedirectToAction("OrderHistory");
        }

        public ActionResult PlaceOrder()
        {
            if (Session["cart"] != null)
            {
                string j = (string)Session["cart"];
                List<CartProduct> d = new List<CartProduct>();
                d = new JavaScriptSerializer().Deserialize<List<CartProduct>>(j);

                var db = new AmaderBazarEntities();

                foreach (var c in d)

                {
                    var cp = (from i in db.Products
                              where i.PID == c.PID
                              select i).FirstOrDefault();
                    cp.Qty -= c.Qty;
                    db.SaveChanges();
                }

                DateTime localDate = DateTime.Now;
                string cultureName = "en-US";
                var culture = new CultureInfo(cultureName);

                Transaction t = new Transaction()
                {
                    UID = Session["UID"].ToString(),
                    TAmount = d.Select(i => i.Price).Sum(),
                    TDetials = Session["cart"].ToString(),
                    Status = "In Processing",
                    TDate = localDate.ToString(culture)
                };

                db.Transactions.Add(t);
                db.SaveChanges();

                Session.Remove("cart");
                return RedirectToAction("OrderHistory");
            }
            else
                return RedirectToAction("cart");
        }

        public ActionResult OrderHistory()
        {
            var d = new TransactionBind();
            var db = new AmaderBazarEntities();

            var t = db.Transactions.ToList();

            d.Transactions = t;
            d.MapData();

            return View(d);
        }

        public ActionResult UProfile()
        {
            if (Session["UID"] != null)
            {
                var UID = Session["UID"].ToString();
                AmaderBazarEntities db = new AmaderBazarEntities();
                var data = db.Users.FirstOrDefault(u => u.UID == UID);
                ViewBag.UserDetails = data;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditProfile()
        {
            if (Session["UID"] != null)
            {
                var UID = Session["UID"].ToString();
                AmaderBazarEntities db = new AmaderBazarEntities();
                var data = db.Users.FirstOrDefault(u => u.UID == UID);
                ViewBag.UserDetails = data;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfile user)
        {
            if (ModelState.IsValid)
            {
                var db = new AmaderBazarEntities();
                var User = db.Users.Where(x => x.UID == user.UID).FirstOrDefault();
                if (User != null)
                {
                    User.Name = user.Name;
                    User.Email = user.Email;
                    User.Address = user.Address;
                    User.Contact = user.Contact;
                    db.Entry(User).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("UProfile");
        }
    }
}