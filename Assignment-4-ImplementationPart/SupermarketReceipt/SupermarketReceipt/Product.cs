using Newtonsoft.Json;

namespace SupermarketReceipt
{
    public class Product
    {
        public string ProductId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public SpecialDeal? Deal { get; private set; }

        public Product(string productId, decimal unitPrice, SpecialDeal? deal = null)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            Deal = deal;
        }

        public void ApplySpecialDeal(SpecialDeal deal)
        {
            Deal = deal;
        }
    }
}