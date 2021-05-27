using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using PagedList;
using PagedList.Mvc;
using Final.Auth;
namespace Final.Controllers
{
    [CustomAuth]
    public class Items1Controller : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Items1
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Brand).Include(i => i.Category).Include(i => i.Subcategory);
            return View(items.ToList());
        }

        // GET: Items1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items1/Create
        public ActionResult Create()
        {
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name");
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SubCategory_Id = new SelectList(db.Subcategories, "Id", "Name");
            return View();
        }

        // POST: Items1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Name,Size,Color,Unit,Price,Discount,Brand_Name,Category_Id,SubCategory_Id")] Item item, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    item.Image = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(item.Image, 0, imageFile.ContentLength);
                }
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SubCategory_Id = new SelectList(db.Subcategories, "Id", "Name");
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name", item.Brand_Name);
            return View(item);
        }

        // GET: Items1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name", item.Brand_Name);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name", item.Category_Id);
            ViewBag.SubCategory_Id = new SelectList(db.Subcategories, "Id", "Name", item.SubCategory_Id);
            return View(item);
        }

        // POST: Items1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Name,Size,Color,Unit,Price,Discount,Brand_Name,Image,Category_Id,SubCategory_Id")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name", item.Brand_Name);
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name", item.Category_Id);
            ViewBag.SubCategory_Id = new SelectList(db.Subcategories, "Id", "Name", item.SubCategory_Id);
            return View(item);
        }

        // GET: Items1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
