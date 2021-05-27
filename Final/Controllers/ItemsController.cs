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
    
    public class ItemsController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Items
        public ActionResult Index2(string search,int ? page,string sort)
        {
            var item = db.Items.Include(b => b.Brand).AsQueryable();

            //System.Diagnostics.Debug.WriteLine(sort);
            item = item.Where(x => x.Name.StartsWith(search) || search == null);
            item = item.Include(x => x.Reviews);
            switch (sort)
            {
                case "Name desc":
                    item = item.OrderByDescending(x => x.Name);
                    break;
                case "Price":
                    item = item.OrderByDescending(x => x.Price);
                    break;
                default:
                    item = item.OrderBy(x => x.Name);
                    break;

            }
            return View(item.ToList().ToPagedList(page ?? 1, 8));

        }

        public ActionResult Index(string search,string sort,int? page,string category)
        {
            
            var item = db.Items.Include(b=>b.Brand).AsQueryable();
            item = item.Include(x => x.Category);

            ViewBag.search = search;
            ViewBag.sort = sort;
            System.Diagnostics.Debug.WriteLine("Hello");
            if (sort == null)
            {
                sort = "Name";
            }
            switch (sort) {
                case "Price High to low":
                    item = item.OrderByDescending(x => x.Price);
                    break;
                case "Price Low to high":
                    item = item.OrderBy(x => x.Price);
                    break;
                case "Name":
                    item = item.OrderByDescending(x => x.Name);
                    break;
                

            }
            item = item.Where(x => x.Name.StartsWith(search) || search == null || x.Brand.Name.StartsWith(search));
            item = item.Where(x => x.Category.Name.StartsWith(category)|| category == null);
            //System.Diagnostics.Debug.WriteLine(item.ToString());
            return View(item.ToList().ToPagedList(page ?? 1,8));
        }

        public ActionResult Category(string subcategory,string category,int? page, string search, string sort)
        {
            var item = db.Items.Include(x=>x.Subcategory).Include(x=>x.Category).AsQueryable();
            if (sort == null)
            {
                sort = "Name";
            }
            switch (sort)
            {
                case "Price High to low":
                    item = item.OrderByDescending(x => x.Price);
                    break;
                case "Price Low to high":
                    item = item.OrderBy(x => x.Price);
                    break;
                case "Name":
                    item = item.OrderByDescending(x => x.Name);
                    break;
            }
            item = item.Where(x => x.Category.Name.StartsWith(category) || category == null);
            item = item.Where(x => x.Subcategory.Name.StartsWith(subcategory) || subcategory == null);
            item = item.Where(x => x.Name.StartsWith(search) || search == null || x.Brand.Name.StartsWith(search));
            ViewBag.category = category;
            ViewBag.subcategory = subcategory;
            ViewBag.search = search;
            ViewBag.sort = sort;
            return View(item.ToList().ToPagedList(page ?? 1,8));
        }
        
        public void  Demo()
        {
            var item = db.Reviews.SqlQuery("Select avg(rate) from Reviews group by Item_Id").ToList();
            //System.Diagnostics.Debug.WriteLine("Hello");
        }
        

        // GET: Items/Details/5
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

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name");
            ViewBag.Category_Id = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SubCategory_Id = new SelectList(db.Subcategories, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Name,Size,Color,Unit,Price,Discount,Brand_Name,Category_Id,SubCategory_Id")] Item item,HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if(imageFile != null)
                {
                    item.Image = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(item.Image, 0, imageFile.ContentLength);
                }
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name", item.Brand_Name);
            return View(item);
        }

        // GET: Items/Edit/5
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
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Name,Size,Color,Unit,Price,Discount,Brand_Name,Image")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brand_Name = new SelectList(db.Brands, "Id", "Name", item.Brand_Name);
            return View(item);
        }

        // GET: Items/Delete/5
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
        public ActionResult Cart(int? id,int? page)
        {            
            var cart = db.Carts.Where(x => x.User_Id == id).Include(x=>x.Item).ToList();           
            return View(cart.ToPagedList(page ?? 1, 8));
        }
        public ActionResult DeleteCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            //System.Diagnostics.Debug.WriteLine(cart.Id.ToString());
            if (cart == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Carts.Remove(cart);
                db.SaveChanges();
                return Json(data: new { success = "true", message = "Removed from cart!!!" }, JsonRequestBehavior.AllowGet);
            }
            
            
        }
        public ActionResult AddToCart(int? id)
        {
            //System.Diagnostics.Debug.WriteLine("Hello");
            //System.Diagnostics.Debug.WriteLine(id);
            if (id!=null)
            {        
                Cart cart = new Cart();
                var item = db.Items.Find(id);
                var user = db.Users.Find(Convert.ToInt32(Session["UserID"]));
                //System.Diagnostics.Debug.WriteLine(item.Id);
                if (item != null && user != null)
                {
                    Console.WriteLine(item.Id);
                    cart.Item_Id = item.Id;
                    cart.User_Id = user.Id;
                    cart.Details = item.Name;
                    db.Carts.Add(cart);
                    db.SaveChanges();
                    return Json(data:new { success = "true", message = "Item added to cart!" }, JsonRequestBehavior.AllowGet);
                }
                else if(user==null)
                {
                    return Json(data:new { success = "false", message = "Please Login first" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: new { success = "error", message = "Error adding item" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(data:new { error="true",message = "Something went wrong!" }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult GetItemDetails(int? id)
        {
            //System.Diagnostics.Debug.WriteLine(id);
            Item item = db.Items.Find(id);
            //System.Diagnostics.Debug.WriteLine(item);
            return Json(data: new { item.Id }, JsonRequestBehavior.AllowGet);
        }
     
        public ActionResult PlaceOrder(string m)
        {
            var money = Convert.ToDouble(m);
            System.Diagnostics.Debug.WriteLine("----------------------------------Hello--------------------------------------");
            System.Diagnostics.Debug.WriteLine(money);
            Order o = new Order();
            Order_Details od = new Order_Details();
            Random r = new Random();
            int user_id = (int)Session["UserID"];
            o.amount = money;
            o.Users_Id =user_id;
            o.Order_Date = DateTime.Now;
            o.Number = DateTime.Now.ToString("yyyyMMdd") +r.Next().ToString() + o.Users_Id.ToString();
            db.Orders.Add(o);
            db.SaveChanges();
            var cart = db.Carts.Where(x => x.User_Id == user_id).ToList();
            for (int x = 0;x< cart.Count;x++)
            {
                
                od.Order_Id = o.Id;
                od.Item_Id = cart[x].Item_Id;
                od.Quantity = 1;
                od.Amount =cart[x].Item.Price;
                db.Order_Details.Add(od);
                db.SaveChanges();
                db.Carts.Remove(cart[x]);
            }
            db.SaveChanges();

            return View();
        }
        // POST: Items/Delete/5
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
