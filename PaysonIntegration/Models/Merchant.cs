using System;

namespace PaysonIntegration.Models
{
    public class Merchant
    {
        public Uri CheckoutUri { get; set; }

        public Uri ConfirmationUri { get; set; }

        public Uri NotificationUri { get; set; }

        public Uri ValidationUri { get; set; }

        public Uri TermsUri { get; set; }

        public string Reference { get; set; }
    }
}