using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketPriceCalculator.DataEntity;
using BasketPriceCalculator.Repositories;

namespace BasketPriceCalculator.Service
{
    public class PriceCalculatorService
    {
        private IPriceRepository _priceRepository;
        private IPromotionRepository _promotionRepository;
        private bool _basketChanged = false;
        private List<Price> _lineItems = new List<Price>();
        private double? _subTotal;
        private double? _total;
        private string _promoText;
        private const string NO_PROMO_MSG = "(No offers available)";

        public PriceCalculatorService(IPriceRepository priceRepository, IPromotionRepository promoRepository)
        {
            _priceRepository = priceRepository;
            _promotionRepository = promoRepository;
        }

        public void SetBasket(List<string> items)
        {
            try
            {
                if (items == null)
                    throw new ArgumentNullException("Items");
                if (items.Count == 0)
                    throw new ArgumentOutOfRangeException("Items", " Basket item count must be atleast one");
                _basketChanged = true;

                foreach (string item in items.Distinct())
                {
                    var lineitem = _priceRepository.Get(item);
                    if (null == lineitem)
                        throw new System.IO.InvalidDataException(string.Format("Item {0} does not exist", item));
                    lineitem.Quantity = items.Count(i => i.Equals(item));
                    LineItems.Add(lineitem);
                }
            }
            catch
            {
                throw;
            }
        }

        public void CalculatePromotions()
        {
            //set sub total and total first
            calculateSubTotal();

            var allPromos = _promotionRepository.GetAll();
            //filter by date range
            allPromos = allPromos.Where(p => ((p.ValidFrom <= DateTime.Now) && (DateTime.Now <= p.ValidTo))).ToList();
            if (allPromos == null)
            {
                _promoText = NO_PROMO_MSG;
                return;
            }
            StringBuilder promotext = new StringBuilder();
            foreach (Promotion reward in allPromos)
            {
                var promotionDiscount = 0.0;

                var buckets = reward.BucketList;
                bool isValidBucket = !buckets.Select(b => b.ProductId).Except(_lineItems.Select(li => li.ProductId)).Any();
                if (isValidBucket)
                {
                    foreach (Bucket bucket in buckets)
                    {
                        if (isValidBucket)
                        {
                            var lineItem = _lineItems.FirstOrDefault(li => li.ProductId.Equals(bucket.ProductId));     
                            if (bucket.Count <= lineItem.Quantity)
                            {
                                double absoluteDiscount = 0;

                                switch (bucket.Operator)
                                {
                                    case DiscountOperator.Fraction:
                                        if (bucket.Discount > 1.0) throw new System.IO.InvalidDataException("Invalid promotion");
                                        absoluteDiscount = lineItem.LinePrice * bucket.Discount;
                                        break;
                                    case DiscountOperator.Absolute:
                                        absoluteDiscount = bucket.Discount;
                                        if (absoluteDiscount > lineItem.LinePrice) throw new System.IO.InvalidDataException("Invalid promotion");
                                        break;
                                }
                                _total = _total - absoluteDiscount;
                                promotionDiscount = +absoluteDiscount;
                            }
                            else
                                isValidBucket = false;

                        }
                    }
                    if (isValidBucket) promotext.Append(string.Format("{0}- {1} , ", reward.Description, promotionDiscount.ToString("c")));
                }


            }
            _promoText = promotext.Length > 0 ? promotext.ToString() : NO_PROMO_MSG;


        }

        public List<Price> LineItems
        {
            get
            {
                return _lineItems;
            }

        }

        public string PromotionText
        {

            get
            {

                return _promoText;
            }
        }
        public double Total { get { return _total.HasValue ? _total.Value : 0; } }

        public double SubTotal
        {
            get
            {
                if (!_subTotal.HasValue || _basketChanged)
                    calculateSubTotal();
                return _subTotal.Value;
            }
        }
        private void calculateSubTotal()
        {
            _subTotal = LineItems.Sum(l => (l.LinePrice * l.Quantity));
            _total = _subTotal;
            _basketChanged = false;
        }
    }
}
