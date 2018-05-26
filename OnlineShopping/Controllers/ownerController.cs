using OnlineShopping.Models;
using OnlineShopping.ViewModels;
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
        [AllowAnonymous]
        [HttpGet]
        public ActionResult addOwner()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult addOwner(UserOwnerViewModel uovm)
        {
            int count = db.users.Count();
            count++;
            string id = "U";

            if (count < 10) id += "00" + count.ToString();
            else if (count < 100) id += "0" + count.ToString();
            else if (count < 1000) id += count.ToString();

            int count2 = db.shopOwners.Count();
            count++;
            string id2 = "S";

            if (count < 10) id2 += "00" + count.ToString();
            else if (count < 100) id2 += "0" + count.ToString();
            else if (count < 1000) id2 += count.ToString();

            uovm.user.role = "Owner";
            uovm.user.userID = id;
            uovm.shopOwner.userID = id;
            uovm.shopOwner.shopID = id2;

            db.users.Add(uovm.user);
            db.shopOwners.Add(uovm.shopOwner);
            db.SaveChanges();
            return View();
        }
    }
}