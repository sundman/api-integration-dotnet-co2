using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;
using PaysonIntegrationCO2.Models;
using PaysonIntegrationCO2.Models.Enums;

namespace PaysonIntegrationCO2
{
    public class ApiCaller
    {
        
        private string ApiUrl { get; set; }

        private string ApiKey { get; }

        private string AgentId { get; }


        private string CheckoutsUrl => $"{ApiUrl}/checkouts";

        private string AccountUrl => $"{ApiUrl}/accounts";


        /// <summary>
        /// Construcor for ApiCaller
        /// </summary>
        /// <param name="agentId">The id of the merchant.</param>
        /// <param name="apiKey">The api key of the merchant.</param>
        /// <param name="inTestMode">Test mode enabled</param>
        public ApiCaller(string agentId, string apiKey, bool inTestMode = false)
        {
            AgentId = agentId;
            ApiKey = apiKey;
            ApiUrl = $"https://{(inTestMode ? "test-" : "")}api.payson.se/2.0/";
        }


        /// <summary>
        /// Ignore this function! Used to set up API url for local debugging
        /// </summary>
        public void SetApiUrl(string apiUrl)
        {
            if (!string.IsNullOrEmpty(apiUrl))
            {
                ApiUrl = apiUrl;
            }
        }

        /// <summary>
        /// Method to add a new checkout.
        /// </summary>
        /// <param name="checkout">The checkout object that will get added.</param>
        /// <returns>Am URL to the location of the newly added checkout object.</returns>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public string NewCheckout(Checkout checkout)
        {
            var requestBody = JsonConvert.SerializeObject(checkout);

            var response = ApiRequest("Post", CheckoutsUrl, requestBody);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new WebException("Unexpected status code.", null, WebExceptionStatus.Success, response);
            }

            var checkoutLocation = response.Headers["Location"];

            return checkoutLocation;
        }

        /// <summary>
        /// Get a list of checkouts created by the merchant.
        /// </summary>
        /// <param name="page">The page to get.</param>
        /// <param name="status">Filter checkouts by status.</param>
        /// <returns>The list of checkouts created by the merchant.</returns>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public CheckoutList GetCheckoutList(int page = 1, CheckoutStatus? status = null)
        {
            var url = $"{CheckoutsUrl}?page={page}&status={status}";

            var response = ApiRequest("Get", url, string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unexpected status code.", null, WebExceptionStatus.Success, response);
            }

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

            var checkoutList = JsonConvert.DeserializeObject<CheckoutList>(responseBody);

            return checkoutList;
        }

        /// <summary>
        /// Get a checkout by its id.
        /// </summary>
        /// <param name="id">The id of the checkout to get.</param>
        /// <returns>The checkout with the given id</returns>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public Checkout GetCheckout(Guid id)
        {
            var checkoutUrl = CheckoutsUrl + "/" + id;

            return GetCheckout(checkoutUrl);
        }

        /// <summary>
        /// Get a agent information by credentials.
        /// </summary>
        /// <returns>The agent dto</returns>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public Account Validate()
        {
            var response = ApiRequest("Get", AccountUrl + "/", string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unexpected status code.", null, WebExceptionStatus.Success, response);
            }

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

            var agent = JsonConvert.DeserializeObject<Account>(responseBody);
            return agent;
        }

        /// <summary>
        /// Get a checkout by its location.
        /// </summary>
        /// <param name="url">The location of the checkout.</param>
        /// <returns>The checkout found at the given location.</returns>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public Checkout GetCheckout(string url)
        {
            var response = ApiRequest("Get", url, string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unexpected status code.", null, WebExceptionStatus.Success, response);
            }

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

            var checkout = JsonConvert.DeserializeObject<Checkout>(responseBody);

            return checkout;
        }

        /// <summary>
        /// Save changes made to a checkout.
        /// </summary>
        /// <param name="checkout">The changed checkout object.</param>
        /// <exception cref="WebException">Thrown if the web request fails or if the answer is unexpected.</exception>
        public void SaveCheckout(Checkout checkout)
        {
            var requestBody = JsonConvert.SerializeObject(checkout);

            var checkoutUrl = CheckoutsUrl + "/" + checkout.Id;

            var response = ApiRequest("Put", checkoutUrl, requestBody);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Unexpected status code.", null, WebExceptionStatus.Success, response);
            }
        }

        private HttpWebResponse ApiRequest(string method, string url, string requestBody)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", GetAuthorizationHeader());

            if (string.IsNullOrEmpty(requestBody))
            {
                request.ContentLength = 0;
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = Encoding.UTF8.GetByteCount(requestBody);

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }

            return (HttpWebResponse)request.GetResponse();
        }

        private string GetAuthorizationHeader()
        {
            var credentials = $"{AgentId}:{ApiKey}";
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var base64Encoded = Convert.ToBase64String(bytes);

            return $"Basic {base64Encoded}";
        }
    }
}
