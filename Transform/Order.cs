namespace Retail
{
    public class Order
    {
        public string Item { get;  set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public long Revenue
        {
            get
            {
                return Price * Quantity;
            }
        }
    }
}
