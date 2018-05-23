using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.ViewModels
{
    public class CustomerItemViewModel
    {
        public user user { get; set; }
        public List<product> productList { get; set; }

    }
}