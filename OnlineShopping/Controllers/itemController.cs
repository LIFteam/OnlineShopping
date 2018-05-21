using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{
    [AllowAnonymous]
    public class itemController : Controller
    {
        OnlineShoppingDataContext db = new OnlineShoppingDataContext();
        // GET: item
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Owner")]
        public ActionResult addProduct()
        {
            return View();
        }        
        public ActionResult Catalog()
        {            
            var products = db.products.ToList();
            return View(products);
        }
        public ActionResult AddCart(string id)
        {
            var product = (from x in db.products
                           where x.productID == id
                           select x).SingleOrDefault();
            return View("Cart",product);
        }
 
    }
}