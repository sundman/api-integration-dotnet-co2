using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PaysonIntegration.Models;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;

namespace PaysonShop.Models
{
    public class ValidationModel : Checkout
    {
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
    }
}
