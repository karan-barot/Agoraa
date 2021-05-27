using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Final.Auth;
using PagedList;
using PagedList.Mvc;
namespace Final.Controllers
{
    [CustomAuth]
    public class AdminMessagesController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: AdminMessages
        public ActionResult Index()
        {
            var adminMessages = db.AdminMessages.Include(a => a.User);
            return View(adminMessages.ToList());
        }

        // GET: AdminMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMessage adminMessage = db.AdminMessages.Find(id);
            if (adminMessage == null)
            {
                return HttpNotFound();
            }
            return View(adminMessage);
        }

        // GET: AdminMessages/Create
        public ActionResult Create()
        {
            ViewBag.User_Id = new SelectList(db.Users,  "UserName");
            return View();
        }

        [AllowAnonymous]
        // POST: AdminMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult SendMessage(string msg)
        {
            AdminMessage amsg = new AdminMessage();
            //System.Diagnostics.Debug.WriteLine(msg);
            User user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
            string response;
            if (user == null)
            {
                response = "Please Login First!!";
            }
            else
            {
                amsg.User_Id = user.Id;
                amsg.Message_desc = msg;
                amsg.TimeStemp = DateTime.UtcNow.ToString();
                db.AdminMessages.Add(amsg);
                db.SaveChanges();
                response = "Your Message was sent successfully!!!";
            }
            
            return Json(new {message= response }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message_desc,TimeStemp,User_Id")] AdminMessage adminMessage)
        {

            if (ModelState.IsValid)
            {
                db.AdminMessages.Add(adminMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Id = new SelectList(db.Users, "Id", "UserName", adminMessage.User_Id);
            return View(adminMessage);
        }

        // GET: AdminMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMessage adminMessage = db.AdminMessages.Find(id);
            if (adminMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Id = new SelectList(db.Users, "Id", "UserName", adminMessage.User_Id);
            return View(adminMessage);
        }

        // POST: AdminMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message_desc,TimeStemp,User_Id")] AdminMessage adminMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.Users, "Id", "UserName", adminMessage.User_Id);
            return View(adminMessage);
        }

        // GET: AdminMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminMessage adminMessage = db.AdminMessages.Find(id);
            if (adminMessage == null)
            {
                return HttpNotFound();
            }
            return View(adminMessage);
        }

        // POST: AdminMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminMessage adminMessage = db.AdminMessages.Find(id);
            db.AdminMessages.Remove(adminMessage);
            db.SaveChanges();
            return RedirectToAction("Index","Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
