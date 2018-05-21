using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShopping.Controllers
{
    [AllowAnonymous]
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
                          select x).FirstOrDefault();

            if (match == null)
            {
                ViewBag.errMessage = "Invalid Login";
                return View();
            }
            else
            {
                HttpCookie cookie = new HttpCookie("user");
                cookie["userid"] = match.userID;
                Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(user.email, false);
                return RedirectToAction("index", "Home");
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
        }
    }

}