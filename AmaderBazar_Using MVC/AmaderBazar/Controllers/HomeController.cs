using AmaderBazar.BusinessModel;
using AmaderBazar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AmaderBazar.Controllers
{
    public class HomeController : Controller
    {

        static int BID = 2;
        static int SID = 3;
        static int DMID = 4;
        
        public ActionResult Index()
        {
            
            if (Session["UID"] == null)
            {
                ViewBag.Title = "Home";
                return View();
            }
            else if (Session["AccType"].ToString() == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            else if (Session["AccType"].ToString() == "Buyer")
            {
                return RedirectToAction("Home", "Buyer");
            }


            else if (Session["AccType"].ToString() == "Delivery Man")
            {
                return RedirectToAction("Dashboard", "DeliveryMan");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Index(LoginValidation user)
        {
            if (ModelState.IsValid)
            {

                using (AmaderBazarEntities db = new AmaderBazarEntities())
                {
                    var obj = db.Users.Where(a => a.UID.Equals(user.UID) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.AccStatus == "Active")
                        {
                            Session["UID"] = obj.UID.ToString();
                            Session["AccType"] = obj.AccType.ToString();
                            FormsAuthentication.SetAuthCookie(user.AccType, true);
                            Session.Remove("ErrMessage");
                            return RedirectToAction("Index");
                        }
                        else if (obj.AccStatus == "Blocked")
                        {
                            Session["ErrMessage"] = " Your ID is blocked. Please contact the admins for further information".ToString();
                            return RedirectToAction("Index");
                        }
                        else if (obj.AccStatus == "Pending")
                        {
                            Session["ErrMessage"] = " Your ID requires admin approval. Please wait or contact the admins for further information".ToString();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        Session["ErrMessage"] = " Fill Up the credentials correctly".ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Title = "AmaderBazar";
            ViewBag.Message = "Get Products Delivered Instantly";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";
            ViewBag.Message = "Contact Our Admins";

            return View();
        }


        public ActionResult Registration()
        {
            ViewBag.Title = "Registration";
            var db = new AmaderBazarEntities();
            ViewBag.BuyerCount = db.UserIDs.FirstOrDefault(u => u.ID == BID).LastID;
        
            ViewBag.DeliveryManCount = db.UserIDs.FirstOrDefault(u => u.ID == DMID).LastID;

            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationValidation user)
        {
            if (ModelState.IsValid)
            {
                var db = new AmaderBazarEntities();
                UserIDChange(user);
                var userNew = new User();
                userNew.AccType = user.AccType;
                userNew.UID = user.UID;
                userNew.Name = user.Name;
                userNew.Email = user.Email;
                userNew.Address = user.Address;
                userNew.Contact = user.Contact;
                userNew.AccStatus = user.AccStatus;
                userNew.Password = user.Password;
                db.Users.Add(userNew);
                


                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }


                return RedirectToAction("Index");
            }

            return View();
        }


        public static void UserIDChange(RegistrationValidation user)
        {
            if (user.AccType == "Buyer")
            {

                var db = new AmaderBazarEntities();
                var oldUserID = db.UserIDs.FirstOrDefault(u => u.ID == 2);
                db.Entry(oldUserID).CurrentValues.SetValues(new UserID()
                {
                    ID = oldUserID.ID,
                    AccType = oldUserID.AccType,
                    LastID = oldUserID.LastID + 1
                });
                db.SaveChanges();
            }
           
            else if (user.AccType == "Delivery Man")
            {
                var db = new AmaderBazarEntities();
                var oldUserID = db.UserIDs.FirstOrDefault(u => u.ID == 4);
                db.Entry(oldUserID).CurrentValues.SetValues(new UserID()
                {
                    ID = oldUserID.ID,
                    AccType = oldUserID.AccType,
                    LastID = oldUserID.LastID + 1
                });
                db.SaveChanges();
            }
        }

    }

}