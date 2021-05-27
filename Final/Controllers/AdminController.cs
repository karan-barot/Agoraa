using Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using PagedList.Mvc;
using PagedList;
using System.Net;

namespace Final.Controllers
{
    public class AdminController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Users1
        public ActionResult Index1()
        {
            
            var item = new DbEntities();
            item.Carts = db.Carts;
            item.Items = db.Items;

            
            return View(new DbEntities());
        }

        public ActionResult Index()
        {

            var item = new DbEntities();
            
            return View(new DbEntities());
        }

        public ActionResult UserList(int?page)
        {
            var userlist = db.Users.ToList();
            return View(userlist.ToPagedList(page?? 1,8));
        }


    }
}