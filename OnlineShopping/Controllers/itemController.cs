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
        OnlineShoppingDataContext db = new OnlineShoppingDataContext();
        // GET: item
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public ActionResult addProduct()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public ActionResult addProduct(product prod)
        {
            string userid = null;
            if(Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }
            int count = db.products.Count();
            count++;
            string id = "P";

            if (count < 10) id += "00" + count.ToString();
            else if (count < 100) id += "0" + count.ToString();
            else if (count < 1000) id += count.ToString();

            string shopid = (from x in db.shopOwners
                             where x.userID.Equals(userid)
                             select x.shopID).SingleOrDefault();

            product product = new product
            {
                productID = id,
                shopID = shopid,
                productName = prod.productName,
                productContent = prod.productContent,
                productQuantity = prod.productQuantity,
                productPrice = prod.productPrice
            };

            db.products.Add(product);
            db.SaveChanges();

            return RedirectToAction("manageProduct");
        }
        
        public ActionResult manageProduct()
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            string shopid = (from x in db.shopOwners
                             where x.userID.Equals(userid)
                             select x.shopID).SingleOrDefault();

            var product = (from x in db.products
                             where x.shopID.Equals(shopid)
                             select x);
            return View(product);
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
            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var product = (from x in db.products
                        where x.productID.Equals(id)
                        select x).SingleOrDefault();

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(product productUpdate)
        {
            var product = (from x in db.products
                        where x.productID.Equals(productUpdate.productID)
                        select x).SingleOrDefault();

            product.productName = productUpdate.productName;
            product.productContent = productUpdate.productContent;
            product.productQuantity = productUpdate.productQuantity;
            product.productPrice = productUpdate.productPrice;
            
            db.SaveChanges();
            return RedirectToAction("manageProduct");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var product = (from x in db.products
                           where x.productID.Equals(id)
                           select x).SingleOrDefault();

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(product productDelete)
        {

            var product = (from x in db.products
                           where x.productID.Equals(productDelete.productID)
                           select x).SingleOrDefault();

            db.products.Remove(product);
            
            db.SaveChanges();
            return RedirectToAction("manageProduct");
        }


        public ActionResult Payment()
        {
            return View();
        }
        
    }
}