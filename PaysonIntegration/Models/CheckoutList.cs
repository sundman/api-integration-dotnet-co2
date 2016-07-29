using System.Collections.Generic;

namespace PaysonIntegration.Models
{
    public class CheckoutList
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public int Count { get; set; }

        public int TotalCount { get; set; }

        public IList<Checkout> Data { get; set; }
    }
}