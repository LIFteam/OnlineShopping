using OnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.ViewModels
{
    public class UserOwnerViewModel
    {
        public user user { get; set; }
        public shopOwner shopOwner { get; set; }
    }
}