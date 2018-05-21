using OnlineShopping.Models;
using OnlineShopping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    
    public class customerController : Controller
    {
        OnlineShoppingDataContext db = new OnlineShoppingDataContext();
        // GET: customer
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous][HttpGet]
        public ActionResult AddCustomer()
        {
            return View();
        }
        [AllowAnonymous][HttpPost]
        public ActionResult AddCustomer(user cust)
        {
          
            int count = db.users.Count();
            count++;
            string id = "U";

            if (count < 10) id += "00" + count.ToString();
            else if (count < 100) id += "0" + count.ToString();
            else if (count < 1000) id += count.ToString();

            user user = new user
            {
                userID = id,
                name = cust.name,
                contactNo = cust.contactNo,
                email = cust.email,
                role = "Customer",
                password = cust.password
            };

            customer customer = new customer
            {
                userID = id,
                billingAddress = cust.customer.billingAddress,
                shippingAddress = cust.customer.shippingAddress
            };

            

            db.users.Add(user);
            db.customers.Add(customer);
            db.SaveChanges();
            return View();

        }
        public ActionResult ManageCustomer()
        {            
            var user = (from x in db.users
                        where x.role.Equals("customer")
                        select x);
            
            return View(user);
        }
        [HttpGet]
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
            user.customer.billingAddress = userUpdate.customer.billingAddress;
            user.customer.shippingAddress = userUpdate.customer.shippingAddress;

            db.SaveChanges();
            return View(user);
        }
    }
}