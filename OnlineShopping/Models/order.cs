using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class order
    {
        public string id { set; get; }
        public string shippingAddress { set; get; }
        public string billingAddress { set; get; }

    }
}