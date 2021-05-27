using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Final.Models;
using Newtonsoft.Json;

namespace Final.Controllers
{
    public class ReviewsController : Controller
    {
        DbEntities db = new DbEntities();
        public ActionResult Index(int ? id)
        {
            var i = id;
            //System.Diagnostics.Debug.WriteLine(i);
            db.Configuration.ProxyCreationEnabled = false;
            var reviews = db.Reviews.Where(x=>x.Item_Id==id).ToList();
            var username = db.Reviews.Where(x => x.Item_Id == id).Select(u => new { email =u.User.Email,username=u.User.UserName}).ToList(); 

            return Json(new { data=reviews,user=username}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index2()
        {
            var reviews = db.Reviews;
            return View(reviews.ToList());
        }

    
        public ActionResult FeedBack(int  itemId,int  userId,int rate,string feed)
        {
            Review rvw = new Review();
            rvw.Item_Id = itemId;
            rvw.User_Id = userId;
            rvw.Rate = rate;
            rvw.Review1 = feed;
            rvw.TimeStemp = DateTime.UtcNow.ToString();
            System.Diagnostics.Debug.WriteLine(rvw.ToString());
            if (rvw != null)
            {
                db.Reviews.Add(rvw);
                db.SaveChanges();
                return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Demo()
        {
            return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}