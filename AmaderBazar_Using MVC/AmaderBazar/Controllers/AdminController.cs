using AmaderBazar.Authentication;
using AmaderBazar.BusinessModel;
using AmaderBazar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmaderBazar.Controllers
{
    [AdminAccess]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            if (Session["UID"] != null)
            {
                ViewBag.Title = "Dashboard";
                var db = new AmaderBazarEntities();
                ViewBag.DeliveryMen = db.Users.ToList();
                ViewBag.Products = db.Products.ToList();

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Dashboard(Product product)
        {
            if (ModelState.IsValid)
            {
                var db = new AmaderBazarEntities();
                var Category = db.Products.Where(x => x.PID == product.PID).FirstOrDefault();
                var category = db.Categories.Where(
                    x => x.CatName == Category.CatName
                    ).FirstOrDefault();
                if (category != null)
                {
                    category.Items = category.Items - 1;
                    db.Entry(category).State = EntityState.Modified;
                }
                db.Products.Remove(db.Products.Where(x => x.PID == product.PID).FirstOrDefault());
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View();
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

        public ActionResult Panel()
        {
            if (Session["UID"] != null)
            {
                AmaderBazarEntities db = new AmaderBazarEntities();
                var data = db.Users.ToList();
                ViewBag.UserDetails = data;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Panel(ControlPanel user)
        {
            if(ModelState.IsValid)
            {
                var db = new AmaderBazarEntities();
                var User = db.Users.Where(x => x.UID == user.UID).FirstOrDefault();
                var flag = 0;
                if (User.AccStatus == "Pending")
                {
                    Delivery d = new Delivery();
                    d.UID = User.UID;
                    d.Status = "Free";
                    d.NumOfOrders = 0;
                    d.Balance = 0.00;
                    d.DeliveryCharge = 50.00;
                    db.Deliveries.Add(d);
                    flag = 1;
                }
                User.AccStatus = user.AccStatus;

                if (User.AccStatus == "Blocked" && User.AccType == "Delivery Man" && flag != 1)
                {
                    var DeliveryMan = db.Deliveries.Where(x => x.UID == user.UID).FirstOrDefault();
                    DeliveryMan.Status = "Blocked";
                    db.Entry(DeliveryMan).State = EntityState.Modified;
                }
                else if (User.AccStatus == "Active" && User.AccType == "Delivery Man" && flag != 1)
                {
                    var DeliveryMan = db.Deliveries.Where(x => x.UID == user.UID).FirstOrDefault();
                    DeliveryMan.Status = "Free";
                    db.Entry(DeliveryMan).State = EntityState.Modified;
                }
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                flag = 0;
            }
            return RedirectToAction("Panel");
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