using System.Collections.Generic;
using System.Linq;

namespace PaysonShop.Business
{
    public class InMemoryDatabase
    {
        /*
         * This class is used in place of the merchants shop database.
         * When doing an integration to Payson a real database should be used
         * to keep track of all purchases made in the shop.
         */

        private static readonly List<Cart> ShoppingCarts;

        private static int _nextCartId;

        static InMemoryDatabase()
        {
            ShoppingCarts = new List<Cart>();
            _nextCartId = 0;
        }

        public static Cart AddCart(Cart cart)
        {
            cart.Id = _nextCartId;
            _nextCartId++;

            ShoppingCarts.Add(cart);

            return cart;
        }

        public static Cart GetCart(int id)
        {
            return ShoppingCarts.FirstOrDefault(x => x.Id == id);
        }

        public static Cart SaveCart(Cart cart)
        {
            var dbCart = ShoppingCarts.FirstOrDefault(x => x.Id == cart.Id);

            dbCart.Status = cart.Status;
            dbCart.CheckoutId = cart.CheckoutId;
            dbCart.Items = cart.Items;

            return dbCart;
        }
    }
}