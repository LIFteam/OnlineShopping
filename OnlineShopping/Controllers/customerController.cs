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
        // GET: customer
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous][HttpGet]
        public ActionResult addCustomer()
        {
            return View();
        }
        [AllowAnonymous][HttpPost]
        public ActionResult addCustomer(CustomerViewModels cust)
        {
            OnlineShoppingDataContext db = new OnlineShoppingDataContext();
            int count = db.users.Count();
            count++;
            string id = "U";

            if (count < 10) id += "00" + count.ToString();
            else if (count < 100) id += "0" + count.ToString();
            else if (count < 1000) id += count.ToString();

            user user = new user
            {
                userID = id,
                name = cust.u.name,
                contactNo = cust.u.contactNo,
                email = cust.u.email,
                role = "Customer",
                password = cust.u.password
            };

            customer customer = new customer
            {
                userID = id,
                billingAddress = cust.c.billingAddress,
                shippingAddress = cust.c.shippingAddress
            };

            

            db.users.Add(user);
            db.customers.Add(customer);
            db.SaveChanges();
            return View();

        }
        public ActionResult addCustomerConfirm()
        {
            return View();
        }
    }
}