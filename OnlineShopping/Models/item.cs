using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class item
    {
        public string id { set; get;}
        public string name { set; get; }
        public int quantity { set; get; }
        public double price { set; get; }
    }
}