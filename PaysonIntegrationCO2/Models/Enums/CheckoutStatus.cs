
namespace PaysonIntegrationCO2.Models.Enums
{
    public enum CheckoutStatus
    {
        None = 0,
        Created = 1,
        FormsFilled = 2,
        ReadyToPay = 3,
        ProcessingPayment = 4,
        ReadyToShip = 5,
        Shipped = 6,
        PaidToAccount = 7,
        Canceled = 8,
        Credited = 9,
        Expired = 10,
        Denied = 11
    }
}