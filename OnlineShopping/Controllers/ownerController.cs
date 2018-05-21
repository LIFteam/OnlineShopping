using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    public class ownerController : Controller
    {
        OnlineShoppingDataContext db = new OnlineShoppingDataContext();
        // GET: owner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageOwner()
        {
            var user = (from x in db.users
                        where x.role.Equals("Owner")
                        select x);

            return View(user);
        }
        public ActionResult Edit(string id)
        {
            var user = (from x in db.users
                        where x.userID.Equals(id)
                        select x).SingleOrDefault();

            return View(user);
        }        
        [HttpPost]
        public ActionResult Edit(user userUpdate)
        {
            var user = (from x in db.users
                        where x.userID.Equals(userUpdate.userID)
                        select x).SingleOrDefault();

            user.name = userUpdate.name;
            user.contactNo = userUpdate.contactNo;
            user.password = userUpdate.password;

            db.SaveChanges();
            return View(user);
        }
    }
}