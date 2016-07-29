using System;

namespace PaysonIntegration.Models
{
    public class Account
    { 
        public string AccountEmail { get; set; }

        public string Status { get; set; }

        public int MerchantId { get; set; }

        public bool EnabledForInvoice { get; set; }
        public bool EnabledForPaymentPlan { get; set; }

    }
}