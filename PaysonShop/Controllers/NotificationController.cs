using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaysonIntegration;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;
using PaysonShop.Business.Entities;

namespace PaysonShop.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApiCaller _apiCaller;
        private readonly IDatabaseConnection _databaseConnection;

        public NotificationController()
        {
            var paysonMerchantId = ConfigurationManager.AppSettings["PaysonMerchantId"];
            var paysonApiKey = ConfigurationManager.AppSettings["PaysonApiKey"];

            _apiCaller = new ApiCaller(paysonMerchantId, paysonApiKey, true);
            _apiCaller.SetApiUrl(ConfigurationManager.AppSettings["PaysonRestUrl"]);
            _databaseConnection = new InMemoryDatabaseConnection();
        }

        [HttpPost]
        public void Index(int id)
        {
            var cart = _databaseConnection.Get(id);
            var checkout = _apiCaller.GetCheckout(cart.CheckoutId);

            switch (checkout.Status)
            {
                case CheckoutStatus.Created:
                case CheckoutStatus.ReadyToPay:
                case CheckoutStatus.ProcessingPayment:
                    cart.Status = CartStatus.Created;
                    break;
                case CheckoutStatus.ReadyToShip:
                    cart.Status = CartStatus.Paid;
                    break;
                case CheckoutStatus.Shipped:
                case CheckoutStatus.PaidToAccount:
                    cart.Status = CartStatus.Shipped;
                    break;
                case CheckoutStatus.Canceled:
                case CheckoutStatus.Expired:
                    cart.Status = CartStatus.Shipped;
                    break;
                case CheckoutStatus.Credited:
                    cart.Status = CartStatus.Credited;
                    break;
            }

            _databaseConnection.Save(cart);
        }

    }
}
