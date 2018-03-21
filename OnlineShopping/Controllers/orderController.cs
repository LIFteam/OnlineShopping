using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    public class orderController : Controller
    {
        // GET: order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult addOrder()
        {
            return View();
        }
    }
}