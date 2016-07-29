using System;

namespace PaysonIntegration.Models
{
    public class History
    {
        public DateTime? Created { get; set; }

        public DateTime? ReadyToPay { get; set; }

        public DateTime? ReadyToShip { get; set; }

        public DateTime? Shipped { get; set; }

        public DateTime? PaidToAccount { get; set; }

        public DateTime? Canceled { get; set; }

        public DateTime? Expired { get; set; }

        public DateTime? Denied { get; set; }
    }
}