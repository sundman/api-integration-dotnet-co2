using System;
using PaysonIntegrationCO2.Models.Enums;

namespace PaysonIntegrationCO2.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }

        public decimal DiscountRate { get; set; }

        public string Ean { get; set; }

        public Uri ImageUri { get; set; }

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public string Reference { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TotalPriceExcludingTax { get; set; }

        public decimal TotalPriceIncludingTax { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal CreditedAmount { get; set; }

        public ItemType Type { get; set; }

        public decimal UnitPrice { get; set; }

        public Uri Uri { get; set; }

        public Item()
        {
            Type = ItemType.Physical;
        }
    }
}
