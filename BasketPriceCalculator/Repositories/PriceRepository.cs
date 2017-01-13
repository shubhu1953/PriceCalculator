using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketPriceCalculator.DataEntity;

namespace BasketPriceCalculator.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private List<Price> _prices;
        public PriceRepository()
        {
            _prices = new List<Price>()
            {new Price(1, "Apples", Unit.Bag, 1.00),
            new Price(2, "Milk", Unit.Bottle, 1.30),
            new Price(3, "Soup", Unit.Tin, 0.65),
            new Price(4, "Bread", Unit.Loaf, 0.80)};

        }
        public Price Get(string name)
        {
            return _prices.FirstOrDefault(p => p.Description.Equals(name));
        }

        public List<Price> GetAll()
        {
            return _prices;
        }

        public void Delete(Price entity)
        {
            throw new NotImplementedException();
        }
    }
}
