using System.Collections.Generic;
using PaysonIntegrationCO2.Models.Enums;

namespace PaysonIntegrationCO2.Models
{
    public class Order
    {
        public Currency Currency { get; set; }

        public decimal TotalFeeExcludingTax { get; set; }

        public decimal TotalFeeIncludingTax { get; set; }

        public decimal TotalPriceExcludingTax { get; set; }

        public decimal TotalPriceIncludingTax { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public IList<Item> Items { get; set; }

        public Order()
        {
            Items = new List<Item>();
        }
    }
}