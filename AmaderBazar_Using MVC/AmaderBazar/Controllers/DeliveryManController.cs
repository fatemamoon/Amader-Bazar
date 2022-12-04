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
    [DeliveryManAccess]
    public class DeliveryManController : Controller
    {
        // GET: DeliveryMan
        public ActionResult Dashboard()
        {
            var db = new AmaderBazarEntities();
            var bind = new DeliveryManBind();
            string UID = Session["UID"].ToString();
            TransactionBind bindTransaction = new TransactionBind();

            var t = (from i in db.Transactions
                     where i.Status == "In Processing" && (i.DMID == null || i.DMID == UID) 
                     select i).ToList();

            bindTransaction.Transactions = t;
            bindTransaction.MapData();
            bind.Processing = bindTransaction;

            t = (from i in db.Transactions
                 where i.Status == "Delivered" && i.DMID == UID
                 select i).ToList();

            bindTransaction = new TransactionBind();
            bindTransaction.Transactions = t;
            bindTransaction.MapData();
            bind.Delivered = bindTransaction;


            var user = (from i in db.Deliveries
                        where i.UID == UID
                        select i).FirstOrDefault();
            bind.UserInfo = user;
            bind.Rate();
            var x = bind.IncreasingRate;

            return View(bind);
        }

        public ActionResult Active(int id)
        {
            var db = new AmaderBazarEntities();
            var update = (from i in db.Transactions
                          where i.TID == id
                          select i).FirstOrDefault();
            update.DMID = Session["UID"].ToString();
            db.SaveChanges();

            var updateDM = (from i in db.Deliveries
                            where i.UID == update.DMID
                            select i).FirstOrDefault();
            updateDM.Status = "Busy";
            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        public ActionResult Delivered(int id)
        {
            var db = new AmaderBazarEntities();
            var update = (from i in db.Transactions
                          where i.TID == id
                          select i).FirstOrDefault();
            update.DMID = Session["UID"].ToString();
            update.Status = "Delivered";
            db.SaveChanges();

            var updateDM = (from i in db.Deliveries
                      where i.UID == update.DMID
                      select i).FirstOrDefault();
            updateDM.Status = "Free";
            updateDM.NumOfOrders++;
            if (updateDM.NumOfOrders % 5 == 0)
                updateDM.DeliveryCharge += 2;
            updateDM.Balance += updateDM.DeliveryCharge;
            db.SaveChanges();

            return RedirectToAction("Dashboard");
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

        public ActionResult Cancel(int id)
        {
            var db = new AmaderBazarEntities();
            var update = (from i in db.Transactions
                          where i.TID == id
                          select i).FirstOrDefault();

            var updateDM = (from i in db.Deliveries
                            where i.UID == update.DMID
                            select i).FirstOrDefault();
            updateDM.Status = "Free";
            update.DMID = null;

            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }

    }
}