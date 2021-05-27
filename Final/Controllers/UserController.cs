using Final.Auth;
using Final.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace Final.Controllers
{
    public class UserController : Controller
    {
        DbEntities db = new DbEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Final.Models.User userModel)
        {
                Session["Visit"] = 0;
            
                var userDetail = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                if(userDetail == null)
                {
                    Session.Abandon();
                    return View("Index", userModel);
                }
                else
                {
                    Session["UserID"] = userDetail.Id;
                    Session["UserName"] = userDetail.UserName;
                    Session["UserEmail"] = userDetail.Email;
                    Session["UserRole"] = userDetail.Role;
                    Session["Visit"] = (int)Session["Visit"] + 1;
                    VisitCount((int)Session["Visit"]);
                    if (userDetail.Role == "A")
                    {
                        FormsAuthentication.SetAuthCookie(userName:userDetail.UserName,true);
                        return RedirectToAction("Index", "Admin");
                    }
                    else if(userDetail.Role == "U")
                    {
                        FormsAuthentication.SetAuthCookie(userName:userDetail.UserName,true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "User");
                    }
                }

            
        }
        public void VisitCount(int i)
        {
            Visit visit = new Visit();
            visit.Visit_Count = i;

            visit.Visit_Time = DateTime.UtcNow;

            db.Visits.Add(visit);
            db.SaveChanges();

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult LogOut()
        {
            int userId = (int)Session["UserID"];
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "UserName,Email,Password")] User usermodel)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    usermodel.Role = "U";
                    db.Users.Add(usermodel);
                    db.SaveChanges();

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return Json(new { message = "Fail" }, JsonRequestBehavior.AllowGet);
                }
            }
            
            else{
                return Json(new { message = "Error" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [CustomAuth]
        [HttpGet]
        public ActionResult UserProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            User user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            var order = db.Orders.Where(x => x.Users_Id == user.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            
            return View(user);
        }

    }
}