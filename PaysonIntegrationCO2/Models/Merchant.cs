using System;

namespace PaysonIntegrationCO2.Models
{
    public class Merchant
    {
        public Uri CheckoutUri { get; set; }

        public Uri ConfirmationUri { get; set; }

        public Uri NotificationUri { get; set; }

        public Uri ValidationUri { get; set; }

        public Uri TermsUri { get; set; }

        public string Reference { get; set; }

        public string PartnerId{ get; set; }
    }
}