using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.ViewModels
{
    public class OrderProductViewModels
    {
        public product product { get; set; }
        public order order { get; set; }
    }
}