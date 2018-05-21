using OnlineShopping.Models;
using OnlineShopping.ViewModels;
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
        
        static List<product> productList = new List<product>();
               
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
        [HttpGet]
        public ActionResult AddCart(string id)
        {
            var product = (from x in db.products
                           where x.productID == id
                           select x).SingleOrDefault();
            
            bool exist = false;
            foreach(var item in productList)
            {
                if (item.productID.Equals(id))
                {
                    exist = true;
                    item.productQuantity++;
                }
            }
            if(exist == false)
            {
                product.productQuantity = 1;
                productList.Add(product);
            }

            return View(productList);
        }
        [HttpPost]
        public ActionResult AddCart()
        {

            return View();
        }

        public ActionResult ConfirmOrder()
        {


            CustomerItemViewModel vm = new CustomerItemViewModel()
            {
                user =
            };
            return View(productList);
        }
        public ActionResult Payment(List<product> product)
        {
            return View();
        }
        
    }
}