using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PaysonIntegration;
using PaysonIntegration.Models;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;
using PaysonShop.Models;

namespace PaysonShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApiCaller _apiCaller;
        private readonly IDatabaseConnection _databaseConnection;

        public CheckoutController()
        {
            var paysonMerchantId = ConfigurationManager.AppSettings["PaysonMerchantId"];
            var paysonApiKey = ConfigurationManager.AppSettings["PaysonApiKey"];

            _apiCaller = new ApiCaller(paysonMerchantId, paysonApiKey, true);
            _apiCaller.SetApiUrl(ConfigurationManager.AppSettings["PaysonRestUrl"]);
            
            _databaseConnection = new InMemoryDatabaseConnection();
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var cart = _databaseConnection.Get(id);
            var checkout = _apiCaller.GetCheckout(cart.CheckoutId);

            var viewModel = new CheckoutViewModel()
            {
                ShoppingCart = cart,
                CheckoutSnippet = checkout.Snippet,
                ColorScheme = checkout.Gui.ColorScheme
            };

            return View(viewModel);
        }

        [HttpPost]
        public void Update(CheckoutViewModel model)
        {
            var cart = model.ShoppingCart;

            UpdateCheckout(model.ShoppingCart);

            _databaseConnection.Save(cart);
        }

        [HttpPost]
        public string ChangeTheme(CheckoutViewModel model)
        {
            var cart = model.ShoppingCart;

            var checkout = _apiCaller.GetCheckout(cart.CheckoutId);

            checkout.Gui.ColorScheme = model.ColorScheme;

            _apiCaller.SaveCheckout(checkout);

            return checkout.Snippet;
        }

        [HttpPost]
        public string ChangeLocale(CheckoutViewModel model)
        {
            var cart = model.ShoppingCart;

            var checkout = _apiCaller.GetCheckout(cart.CheckoutId);

            checkout.Gui.Locale = model.Locale;

            _apiCaller.SaveCheckout(checkout);

            return checkout.Snippet;
        }

        private void UpdateCheckout(Cart cart)
        {
            var cartItems = cart.Items.ToList();

            var checkout = _apiCaller.GetCheckout(cart.CheckoutId);
            var checkoutItems = checkout.Order.Items.ToList();

            var removedItems = checkoutItems.Where(x => !cartItems.Exists(y => y.Reference == x.Reference));
            var addedItems = cartItems.Where(x => !checkoutItems.Exists(y => x.Reference == y.Reference));

            //Remove deleted items
            checkoutItems.RemoveAll(removedItems.Contains);

            //Add added items
            checkoutItems.AddRange(addedItems.Select(addedItem => new Item()
            {
                Name = addedItem.Name,
                Quantity = addedItem.Quantity,
                UnitPrice = addedItem.UnitPrice,
                Reference = addedItem.Reference
            }));

            //Update all items
            foreach (var checkoutItem in checkoutItems)
            {
                var cartItem = cart.Items.First(x => x.Reference == checkoutItem.Reference);

                checkoutItem.Name = cartItem.Name;
                checkoutItem.Quantity = cartItem.Quantity;
                checkoutItem.UnitPrice = cartItem.UnitPrice;
                checkoutItem.Reference = cartItem.Reference;
            }

            _apiCaller.SaveCheckout(checkout);
        }
    }



}
