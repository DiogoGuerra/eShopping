using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eShopping.Models
{
    public class Cart
    {
        public int ID {get;set;}

        public ICollection<ProductsOrder> productsOrders { get; set; }
    }
}