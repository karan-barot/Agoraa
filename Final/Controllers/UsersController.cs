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
    public class UsersController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Users
        [CustomAuth]
        public ActionResult Index(int? page)
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Country).Include(u => u.State);
            return View(users.ToList().ToPagedList(page??1,8));
        }
        [CustomAuth]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [AllowAnonymous]

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.City_Id = new SelectList(db.Cities, "Id", "Name");
            ViewBag.Country_Id = new SelectList(db.Countries, "Id", "Name");
            ViewBag.State_Id = new SelectList(db.States, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Id,UserName,Email,DOB,Address,Role,Password,Country_Id,City_Id,State_Id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.City_Id = new SelectList(db.Cities, "Id", "Name", user.City_Id);
            ViewBag.Country_Id = new SelectList(db.Countries, "Id", "Name", user.Country_Id);
            ViewBag.State_Id = new SelectList(db.States, "Id", "Name", user.State_Id);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.City_Id = new SelectList(db.Cities, "Id", "Name", user.City_Id);
            ViewBag.Country_Id = new SelectList(db.Countries, "Id", "Name", user.Country_Id);
            ViewBag.State_Id = new SelectList(db.States, "Id", "Name", user.State_Id);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email,DOB,Address,Role,Password,Country_Id,City_Id,State_Id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City_Id = new SelectList(db.Cities, "Id", "Name", user.City_Id);
            ViewBag.Country_Id = new SelectList(db.Countries, "Id", "Name", user.Country_Id);
            ViewBag.State_Id = new SelectList(db.States, "Id", "Name", user.State_Id);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
