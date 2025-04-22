namespace SupermarketReceipt
{
    public class Receipt
    {
        public ICollection<ReceiptLineItem> LineItems { get; }
        public int NumberOfLineItems => LineItems.Count;
        public decimal TotalAmount => LineItems.Sum(l => l.BillableAmount);

        public Receipt
        (
            IEnumerable<Product> items
        )
        {
            var products = items.ToArray();
            
            if (!products.Any())
                throw new ApplicationException("Cannot generate a receipt without items.");
            
            LineItems = new List<ReceiptLineItem>();
            
            var itemGroups = products.GroupBy(itm => itm.ProductId);

            foreach (var itemGroup in itemGroups)
            {
                var product = itemGroup.First();
                var productId = product.ProductId;
                var quantity = itemGroup.Count();
                var price = CalculatePrice(product, quantity);
                var unitPrice = price.UnitPrice;
                var discount = price.Discount;
                
                LineItems.Add
                (
                    new ReceiptLineItem
                    (
                        productId,
                        unitPrice,
                        quantity,
                        discount
                    )
                );
            }
        }

        public ReceiptLineItem LineItem
        (
            string productId
        )
        {
            return LineItems.First(l => l.Description == productId);
        }

        private static Price CalculatePrice
        (
            Product product,
            int quantity
        )
        {
            var unitPrice = product.UnitPrice;
            decimal discount = 0;

            if (product.Deal != null)
            {
                discount = product.Deal.Discount(quantity, product.UnitPrice);
            }

            return new Price(unitPrice, discount);
        }
        
        private class Price
        {
            public decimal UnitPrice { get; }
            public decimal Discount { get; }
            
            public Price
            (
                decimal unitPrice,
                decimal discount
            )
            {
                UnitPrice = unitPrice;
                Discount = discount;
            }
        }

        public class ReceiptLineItem
        {
            public string Description { get; }
            public decimal UnitPrice { get; }
            public int Quantity { get; }
            public decimal Discount { get; }

            public decimal BillableAmount => CalculateBillableAmount(UnitPrice, Quantity, Discount);
            public ReceiptLineItem
            (
                string description,
                decimal unitPrice,
                int quantity,
                decimal discount
            )
            {
                Description = description;
                UnitPrice = unitPrice;
                Quantity = quantity;
                Discount = discount;
            }
            
            private static decimal CalculateBillableAmount(decimal unitPrice, int quantity, decimal discountPercent)
            {
                var totalPrice = unitPrice * quantity;
                var discountAmount = totalPrice * (discountPercent / 100);
                return totalPrice - discountAmount;
            }
        }
    }
}