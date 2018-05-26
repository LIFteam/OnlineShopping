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

            string shopid = (from x in db.shopOwners
                             where x.userID.Equals(userid)
                             select x.shopID).SingleOrDefault();
            int i = 1;
            int gap = 0;
            if (count == 0)
            {
                id = "P001";
            }
            else
            {
                var pro = (from x in db.products
                           orderby x.productID
                           select x);
                
                foreach (var item in pro)
                {
                    string temp = null;
                    temp = item.productID;
                    temp = temp.Substring(1);
                    int temp2 = int.Parse(temp);
                    
                    if(temp2 != i)
                    {
                        gap = i;
                        break;
                    }
                    i++;
                }
            }
            if(gap != 0)
            {
                if (gap < 10) id += "00" + gap.ToString();
                else if (gap < 100) id += "0" + gap.ToString();
                else if (gap < 1000) id += gap.ToString();
            }
            else
            {
                if (count < 10) id += "00" + count.ToString();
                else if (count < 100) id += "0" + count.ToString();
                else if (count < 1000) id += count.ToString();
            }


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
        public ActionResult Cart()
        {            
            return View("AddCart");
        }
        [HttpGet]
        public ActionResult AddCart(string id)
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            var product = (from x in db.products
                           where x.productID == id
                           select x).SingleOrDefault();
            product.productQuantity = 1;
            
            if (Session["cart"] == null)
            {
                List<product> productList = new List<product>();
                productList.Add(product);
                Session["cart"] = productList;
            }
            else
            {
                List<product> productList = (List<product>)Session["cart"];
                bool exist = false;
                foreach(var item in productList)
                {
                    if (item.productID.Equals(product.productID))
                    {
                        exist = true;
                        item.productQuantity++;
                        item.productPrice*=2;
                    }                    
                }
                if(exist == false)
                {
                    productList.Add(product);
                }

                Session["cart"] = productList;
            }
            return View();
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
        [HttpGet]
        public ActionResult DeleteCart(string id)
        {
            List<product> productList = (List<product>)Session["cart"];

            int count = 0;
            foreach (var item in productList)
            {
                if (item.productID.Equals(id))
                {
                    break;
                }
                count++;
            }
            productList.RemoveAt(count);

            return RedirectToAction("Cart");
        }
    }
}