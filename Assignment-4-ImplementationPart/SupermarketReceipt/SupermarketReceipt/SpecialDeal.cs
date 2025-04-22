using Newtonsoft.Json;

namespace SupermarketReceipt
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class SpecialDeal
    {
        public abstract decimal Discount(int quantity, decimal unitPrice);
    }
}