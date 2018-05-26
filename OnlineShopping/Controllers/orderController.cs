using OnlineShopping.Models;
using OnlineShopping.ViewModels;
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
        OnlineShoppingDataContext db = new OnlineShoppingDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult addOrder()
        {
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
            foreach (var item in orderList)
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


            foreach (var item in productList)
            {
                order temp = new order();
                temp.orderID = id3;
                temp.productID = item.productID;
                temp.userID = userid;
                temp.orderDate = DateTime.Today;
                temp.orderQuantity = item.productQuantity;
                temp.shippingAddress = user.customer.shippingAddress;

                var product = (from x in db.products
                               where x.productID.Equals(item.productID)
                               select x).SingleOrDefault();
                product.productQuantity -= item.productQuantity;

                db.orders.Add(temp);
                db.SaveChanges();
            }
            Session["cart"] = null;
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

        public ActionResult Order()
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            string shopid = (from x in db.shopOwners
                             where x.userID.Equals(userid)
                             select x.shopID).SingleOrDefault();

            var orderList = (from x in db.orders
                             join y in db.products on x.productID equals y.productID
                             where y.shopID.Equals(shopid)
                             select x);

            return View(orderList);
        }

        public ActionResult DetailsCustomer(string id)
        {
            var itemList = (from x in db.orders
                            join y in db.products on x.productID equals y.productID
                            where x.orderID.Equals(id)
                            select new { x, y });

            List<OrderProductViewModels> list = new List<OrderProductViewModels>();

            foreach (var item in itemList)
            {
                OrderProductViewModels opvm = new OrderProductViewModels();
                opvm.order = new order();
                opvm.product = new product();
                opvm.order.orderDate = item.x.orderDate;
                opvm.order.orderID = item.x.orderID;
                opvm.order.orderQuantity = item.x.orderQuantity;
                opvm.order.productID = item.x.productID;
                opvm.order.shippingAddress = item.x.shippingAddress;

                opvm.product.productID = item.y.productID;
                opvm.product.productContent = item.y.productContent;
                opvm.product.productName = item.y.productName;
                opvm.product.productPrice = item.y.productPrice;
                opvm.product.productQuantity = item.y.productQuantity;
                opvm.product.shopID = item.y.shopID;
                list.Add(opvm);
            }

            return View(list);
        }
        public ActionResult DetailsOwner(string id)
        {
            string userid = null;
            if (Request.Cookies["user"] != null)
            {
                userid = Request.Cookies["user"]["userid"];
            }

            string shopid = (from x in db.shopOwners
                             where x.userID.Equals(userid)
                             select x.shopID).SingleOrDefault();

            var itemList = (from x in db.orders
                            join y in db.products on x.productID equals y.productID
                            where x.orderID.Equals(id) && y.shopID.Equals(shopid)
                            select new { x, y });

            List<OrderProductViewModels> list = new List<OrderProductViewModels>();

            foreach (var item in itemList)
            {
                OrderProductViewModels opvm = new OrderProductViewModels();
                opvm.order = new order();
                opvm.product = new product();
                opvm.order.orderDate = item.x.orderDate;
                opvm.order.orderID = item.x.orderID;
                opvm.order.orderQuantity = item.x.orderQuantity;
                opvm.order.productID = item.x.productID;
                opvm.order.shippingAddress = item.x.shippingAddress;

                opvm.product.productID = item.y.productID;
                opvm.product.productContent = item.y.productContent;
                opvm.product.productName = item.y.productName;
                opvm.product.productPrice = item.y.productPrice;
                opvm.product.productQuantity = item.y.productQuantity;
                opvm.product.shopID = item.y.shopID;
                list.Add(opvm);
            }

            return View(list);
        }


        public ActionResult Payment()
        {
            return View();
        }

    }
}