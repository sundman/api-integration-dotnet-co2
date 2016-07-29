using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using PaysonIntegration;
using PaysonIntegration.Models;
using PaysonIntegration.Models.Enums;
using PaysonShop.Business;
using PaysonShop.Business.Entities;
using PaysonShop.Models;

namespace PaysonShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IDatabaseConnection _databaseConnection;

        public ShopController()
        {

            _databaseConnection = new InMemoryDatabaseConnection();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; // trust all certificates
        }

        private string CleanUrl(string url)
        {
            var uri = new Uri(url);
            var clean = uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);
            return clean;
        }

        public ActionResult Index()
        {
            //Create a new cart item which is stored in the shop database.
            var cart = CreateShoppingCart();

            var model = new ShopViewModel
            {
                ApiKey = ConfigurationManager.AppSettings["PaysonApiKey"],
                MerchantId = ConfigurationManager.AppSettings["PaysonMerchantId"],
                ShoppingCart = cart
            };

            
            var checkoutUrl = CleanUrl(Url.Action("Index", "Checkout", new { id = cart.Id }, Request.Url.Scheme));
            var notificationUrl = CleanUrl(Url.Action("Index", "Notification", new { id = cart.Id }, Request.Url.Scheme));
            var confirmationUrl = CleanUrl(Url.Action("Index", "Confirmation", new { id = cart.Id }, Request.Url.Scheme));
            var termsUrl = CleanUrl(Url.Action("Index", "Terms", null, Request.Url.Scheme));

            model.Merchant.CheckoutUri = new Uri(checkoutUrl);
            model.Merchant.NotificationUri = new Uri(notificationUrl);
            model.Merchant.ConfirmationUri = new Uri(confirmationUrl);
            model.Merchant.TermsUri = new Uri(termsUrl);

            model.Order.Currency = Currency.SEK; ;

            model.Customer.Email = "test@payson.se";
            model.Customer.IdentityNumber = "4605092222";
            model.Customer.PostalCode = "99999";
            model.Gui.RequestPhone = false;

            model.Gui.ColorScheme = ColorScheme.Gray;
            model.Gui.Locale = "sv";

            AddDefaultItems(model);

            return View(model);
        }

        public ActionResult GoToCheckout(ShopViewModel model)
        {
            try
            {
                var apiCaller = new ApiCaller(model.MerchantId, model.ApiKey, true);
                apiCaller.SetApiUrl(ConfigurationManager.AppSettings["PaysonRestUrl"]);


                //Send the newly checkout data to Payson
                var location = apiCaller.NewCheckout(model);

                //Get a new checkout data object which is populated with aditional information
                var checkout = apiCaller.GetCheckout(location);

                //Update the cart with items and checkout reference.
                var cart = UpdateShoppingCart(model.ShoppingCart, checkout);

                return RedirectToAction("Index", "Checkout", new RouteValueDictionary() { { "id", cart.Id } });
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;

                string responseBody;

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        throw new NullReferenceException("No response stream found.");
                    }

                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }

                return View("Response", null, responseBody);
            }
        }


        public ActionResult Verify(ValidationModel model)
        {
            try
            {
                var apiCaller = new ApiCaller(model.MerchantId, model.ApiKey, true);
                apiCaller.SetApiUrl(ConfigurationManager.AppSettings["PaysonRestUrl"]);

                //Send the newly checkout data to Payson
                var data = apiCaller.Validate();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;

                string responseBody;

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        throw new NullReferenceException("No response stream found.");
                    }

                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }

                return Json(new JavaScriptSerializer().Deserialize<object>(responseBody), JsonRequestBehavior.AllowGet);

            }
        }

        private Cart CreateShoppingCart()
        {
            var cart = new Cart()
            {
                Status = CartStatus.Created
            };

            return _databaseConnection.Add(cart);
        }

        private Cart UpdateShoppingCart(Cart cart, Checkout checkout)
        {
            cart.CheckoutId = checkout.Id;

            cart.Items = checkout.Order.Items.Select(item => new CartItem()
            {
                Reference = item.Reference,
                Name = item.Name,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }
            ).ToList();

            return _databaseConnection.Save(cart);
        }

        private void AddDefaultItems(ShopViewModel model)
        {
            var item1 = new Item()
            {
                DiscountRate = 0.5m,
                Name = "TestItem1",
                Quantity = 1,
                Reference = "R101",
                TaxRate = (decimal)0.25,
                Type = ItemType.Physical,
                UnitPrice = 500
            };

            var item2 = new Item()
            {
                Name = "TestItem2",
                Quantity = 3,
                Reference = "R102",
                TaxRate = 0.4m,
                Type = ItemType.Physical,
                UnitPrice = 50
            };

            var item3 = new Item()
            {
                Name = "TestItem3",
                Quantity = 2,
                Reference = "R103",
                Type = ItemType.Physical,
                UnitPrice = 20
            };

            var item4 = new Item()
            {
                Name = "TestFee",
                Quantity = 1,
                Reference = "F001",
                TaxRate = 0.25m,
                Type = ItemType.Fee,
                UnitPrice = 100
            };

            model.Order.Items.Add(item1);
            model.Order.Items.Add(item2);
            model.Order.Items.Add(item3);
            model.Order.Items.Add(item4);
        }
    }
}
