using System;
using PaysonIntegration.Models.Enums;

namespace PaysonIntegration.Models
{
    public class Checkout
    {
        public Guid Id { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public string Snippet { get; set; }

        public CheckoutStatus Status { get; set; }

        public Customer Customer { get; set; }

        public Order Order { get; set; }

        public Merchant Merchant { get; set; }

        public Gui Gui { get; set; }

        public History History { get; set; }

        public int? PurchaseId { get; set; }

        public Checkout()
        {
            Customer = new Customer();
            Order = new Order();
            Merchant = new Merchant();
            Gui = new Gui();
            History = new History();
        }
    }
}