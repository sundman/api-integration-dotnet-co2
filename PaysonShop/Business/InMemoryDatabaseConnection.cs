namespace PaysonShop.Business
{
    public class InMemoryDatabaseConnection : IDatabaseConnection
    {
        public Cart Add(Cart cart)
        {
            return InMemoryDatabase.AddCart(cart);
        }

        public Cart Get(int id)
        {
            return InMemoryDatabase.GetCart(id);
        }

        public Cart Save(Cart cart)
        {
            return InMemoryDatabase.SaveCart(cart);
        }
    }
}
