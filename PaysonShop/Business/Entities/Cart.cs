using System;
using System.Collections.Generic;
using PaysonShop.Business.Entities;

namespace PaysonShop.Business
{
    public class Cart
    {
        public int Id { get; set; }

        public Guid CheckoutId { get; set; }

        public CartStatus Status { get; set; }

        public IList<CartItem> Items { get; set; }
    }
}