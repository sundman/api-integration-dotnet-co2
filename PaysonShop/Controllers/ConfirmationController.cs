using System.Configuration;
using System.Web.Mvc;
using PaysonIntegration;
using PaysonShop.Business;
using PaysonShop.Models;

namespace PaysonShop.Controllers
{
    public class ConfirmationController : Controller
    {
        private readonly ApiCaller _apiCaller;
        private readonly IDatabaseConnection _databaseConnection;

        public ConfirmationController()
        {
            var paysonMerchantId = ConfigurationManager.AppSettings["PaysonMerchantId"];
            var paysonApiKey = ConfigurationManager.AppSettings["PaysonApiKey"];

            _apiCaller = new ApiCaller(paysonMerchantId, paysonApiKey, true);
            _apiCaller.SetApiUrl(ConfigurationManager.AppSettings["PaysonRestUrl"]);
            _databaseConnection = new InMemoryDatabaseConnection();
        }

        public ActionResult Index(int id)
        {
            var cart = _databaseConnection.Get(id);
            var checkoutId = cart.CheckoutId;
            var paysonCheckout = _apiCaller.GetCheckout(checkoutId);

            var model = new ConfirmationViewModel { ShoppingCart = cart, ConfirmationSnippet = paysonCheckout.Snippet };

            return View(model);
        }

    }
}
