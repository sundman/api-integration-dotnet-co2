using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PaysonIntegration.Models;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;

namespace PaysonShop.Models
{
    public class ShopViewModel : Checkout
    {
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }

        public Cart ShoppingCart { get; set; }

        public List<SelectListItem> CurrencyOptions
        {
            get
            {
                var values = Enum.GetValues(typeof(Currency));
                var list = new List<SelectListItem>();

                foreach (Currency value in values)
                {
                    var selected = value == Order.Currency;
                    list.Add(new SelectListItem() { Text = value.ToString(), Value = value.ToString(), Selected = selected });
                }

                return list;
            }
        }

        public List<SelectListItem> ColorSchemeOptions
        {
            get
            {
                var values = Enum.GetValues(typeof(ColorScheme));
                var list = new List<SelectListItem>();

                foreach (ColorScheme value in values)
                {
                    var selected = value == Gui.ColorScheme;
                    list.Add(new SelectListItem() { Text = value.ToString(), Value = value.ToString(), Selected = selected });
                }

                return list;
            }
        }

        public List<SelectListItem> LocaleOptions
        {
            get
            {
                var values = new[] { "en", "en-US", "sv", "sv-FI", "sv-SE" };
                var list = values.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();

                return list;
            }
        }


        public List<SelectListItem> CustomerTypeOptions
        {
            get
            {
                var values = Enum.GetValues(typeof(CustomerType));
                var list = new List<SelectListItem>();

                foreach (CustomerType value in values)
                {
                    var selected = value == Customer.Type;
                    list.Add(new SelectListItem() { Text = value.ToString(), Value = value.ToString(), Selected = selected });
                }

                return list;
            }
        }

        public List<SelectListItem> GetItemTypeOptions(ItemType selectedType)
        {
            var values = Enum.GetValues(typeof(ItemType));
            var list = new List<SelectListItem>();

            foreach (ItemType value in values)
            {
                var selected = value == selectedType;
                list.Add(new SelectListItem() { Text = value.ToString(), Value = value.ToString(), Selected = selected });
            }

            return list;
        }
    }
}
