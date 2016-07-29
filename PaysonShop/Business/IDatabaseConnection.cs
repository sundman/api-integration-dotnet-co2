using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaysonShop.Business
{
    public interface IDatabaseConnection
    {
        Cart Add(Cart cart);
        Cart Get(int id);
        Cart Save(Cart cart);
    }
}
