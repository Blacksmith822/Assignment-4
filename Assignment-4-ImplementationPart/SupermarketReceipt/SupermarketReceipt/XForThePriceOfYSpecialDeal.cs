using Newtonsoft.Json;

namespace SupermarketReceipt
{
    public class XForThePriceOfYSpecialDeal : SpecialDeal
    {
        [JsonProperty]
        public decimal Quantity { get; private set; }

        [JsonProperty]
        public decimal ForThePriceOf { get; private set; }

        public XForThePriceOfYSpecialDeal(decimal quantity, decimal forThePriceOf)
        {
            Quantity = quantity;
            ForThePriceOf = forThePriceOf;
        }

        public override decimal Discount(int quantity, decimal unitPrice)
        {
            var numberNotToDiscount = quantity % Quantity;
            var numberToDiscount = Math.Floor(quantity / Quantity);
            var discountedAmount = numberToDiscount * unitPrice * ForThePriceOf;
            var remainingRegularAmount = numberNotToDiscount * unitPrice;
            var totalDiscountedAmount = discountedAmount + remainingRegularAmount;
            var totalRegularAmount = quantity * unitPrice;

            return ((totalRegularAmount - totalDiscountedAmount) / totalRegularAmount) * 100;
        }
    }
}