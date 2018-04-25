using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShopping.Controllers
{
    public class authenticationController : Controller
    {
        // GET: authentication
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user user)
        {
            OnlineShoppingDataContext db = new OnlineShoppingDataContext();

            user match = (from x in db.users
                          where (user.email.Equals(x.email) && user.password.Equals(x.password))
                          select x).SingleOrDefault();

            if (match == null)
            {
                ViewBag.errMessage = match.email;
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(user.name, false);
                return RedirectToAction("index", "Home");
            }

        }
    }
}