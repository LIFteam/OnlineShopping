//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShopping.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class customer
    {

        public string userID { get; set; }
        [Required]
        public string billingAddress { get; set; }
        [Required]
        public string shippingAddress { get; set; }
    
        public virtual user user { get; set; }
    }
}
