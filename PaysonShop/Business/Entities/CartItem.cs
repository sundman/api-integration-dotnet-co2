using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaysonShop.Business.Entities
{
    public class CartItem
    {
        public string Reference { get; set; }

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}