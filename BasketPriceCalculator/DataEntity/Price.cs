namespace BasketPriceCalculator.DataEntity
{
    public class Price
    {
        public Price(int productId, string description, Unit unit, double linePrice)
        {
            ProductId = productId;
            Description = description;
            Unit = unit;
            LinePrice = linePrice;

        }

        public int ProductId { get; set; }
        public string Description { get; set; }
        public Unit Unit { get; set; }
        public double LinePrice { get; set; }
        public int Quantity { get; set; }

    }

    public enum Unit
    {
        Tin,
        Loaf,
        Bottle,
        Bag
    }
}

