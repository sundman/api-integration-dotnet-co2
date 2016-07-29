using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;

namespace PaysonShop.Models
{
    public class CheckoutViewModel
    {
        public Cart ShoppingCart { get; set; }

        public string CheckoutSnippet { get; set; }

        public ColorScheme ColorScheme { get; set; }

        public string Locale { get; set; }

        public List<SelectListItem> ColorSchemeOptions
        {
            get
            {
                var values = Enum.GetValues(typeof(ColorScheme));
                var list = new List<SelectListItem>();

                foreach (ColorScheme value in values)
                {
                    var selected = value == ColorScheme;
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
    }
}