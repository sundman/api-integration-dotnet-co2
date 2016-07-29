using PaysonShop.Business;

namespace PaysonShop.Models
{
    public class ConfirmationViewModel
    {
        public Cart ShoppingCart { get; set; }

        public string ConfirmationSnippet { get; set; }
    }
}