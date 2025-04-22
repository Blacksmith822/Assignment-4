namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly ICatalog _productCatalog;
        private readonly ICollection<Product> _items = new List<Product>();
        public int NumberOfItems => _items.Count;

        public ShoppingCart(ICatalog catalog)
        {
            _productCatalog = catalog;
        }

        public Receipt Receipt()
        {
            if (!_items.Any())
                throw new ApplicationException("Cannot produce receipt for an empty cart.");

            return new Receipt(_items);
        }

        public void RemoveItemByProductId
        (
            string productId
        )
        {
            var itemToRemove = _items.FirstOrDefault(item => item.ProductId == productId);
            
            if (itemToRemove == null)
                throw new ApplicationException("Cannot remove an item that is not in the cart.");
            
            _items.Remove(itemToRemove);
        }

        public void AddItemByProductId
        (
            string productId
        )
        {
            if (!_productCatalog.Contains(productId))
                throw new ApplicationException("Cannot add an item to the cart that is not in the catalog.");
            
            _items.Add(_productCatalog.Product(productId));
        }
    }
}