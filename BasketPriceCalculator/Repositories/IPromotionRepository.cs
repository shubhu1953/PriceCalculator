using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketPriceCalculator.DataEntity;

namespace BasketPriceCalculator.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion, int>
    {
    }
}
