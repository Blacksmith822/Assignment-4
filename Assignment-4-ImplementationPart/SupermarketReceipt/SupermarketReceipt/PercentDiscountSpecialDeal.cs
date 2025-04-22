using Newtonsoft.Json;

namespace SupermarketReceipt
{
    public class PercentDiscountSpecialDeal : SpecialDeal
    {
        [JsonProperty]
        private readonly decimal _percent;

        public PercentDiscountSpecialDeal(decimal percent)
        {
            _percent = percent;
        }

        public override decimal Discount(int quantity, decimal unitPrice)
        {
            return _percent;
        }
    }
}