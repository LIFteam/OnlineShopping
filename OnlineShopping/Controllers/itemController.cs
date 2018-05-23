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

            return View();
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
        public ActionResult ConfirmOrder()
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            var user = (from x in db.users
                        where x.userID.Equals(userid)
                        select x).SingleOrDefault();

            return View(user);
        }
        [HttpPost]
        public ActionResult ConfirmOrder(user user)
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }
            var orderList = (from x in db.orders
                            select x);
            string id = null;

            int row = 0;
            foreach(var item in orderList)
            {
                if (item.orderID != null)
                {
                    id = item.orderID;                    
                    id = id.Substring(1);
                    row = int.Parse(id);
                }
            }
            
            row++;
            string id3 = "O";

            if (row < 10) id3 += "00" + row.ToString();
            else if (row < 100) id3 += "0" + row.ToString();
            else if (row < 1000) id3 += row.ToString();

            List<product> productList = (List<product>)Session["cart"];
            

            foreach(var item in productList)
            {
                order temp = new order();
                temp.orderID = id3;
                temp.productID = item.productID;
                temp.userID = userid;
                temp.orderDate = DateTime.Today;
                temp.orderQuantity = item.productQuantity;
                temp.shippingAddress = user.customer.shippingAddress;

                db.orders.Add(temp);
                db.SaveChanges();
            }

            return RedirectToAction("History");
        }
        public ActionResult History()
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            var order = (from x in db.orders
                         where x.userID.Equals(userid)
                         select x);
            return View(order);            
        }
        public ActionResult Payment()
        {
            return View();
        }
        
    }
}