using Newtonsoft.Json;

namespace SupermarketReceipt
{
    public class Catalog : ICatalog
    {
        private readonly string _filePath;
        private IDictionary<string, Product> _productCatalog;

        public int NumberOfProducts => _productCatalog.Count;

        public bool LoadCatalogExecuted { get; private set; }
        public bool SaveCatalogExecuted { get; private set; }

        public Catalog()
        {
            _filePath = "catalog.json";
            LoadCatalog();
        }

        public bool Contains(string productId)
        {
            LoadCatalog();
            return _productCatalog.ContainsKey(productId);
        }

        public Product Product(string productId)
        {
            if (!Contains(productId))
                throw new ApplicationException($"Product with ID {productId} not found in the catalog.");

            return _productCatalog[productId];
        }

        public void AddProduct(Product product)
        {
            if (Contains(product.ProductId))
                throw new ApplicationException("Cannot add a product that is already in the catalog.");

            _productCatalog[product.ProductId] = product;
            SaveCatalog();
        }

        public void RemoveProduct(string productId)
        {
            if (!Contains(productId))
                throw new ApplicationException("Cannot remove a product that is not in the catalog.");

            _productCatalog.Remove(productId);
            SaveCatalog();
        }
        
        public void ApplySpecialDeal
        (
            SpecialDeal deal,
            string productId
        )
        {
            Product(productId).ApplySpecialDeal(deal);
            SaveCatalog();
        }

        public void Clear()
        {
            _productCatalog = new Dictionary<string, Product>();

            SaveCatalog();
            
            if(File.Exists(_filePath))
                File.Delete(_filePath);
        }

        private void LoadCatalog()
        {
            LoadCatalogExecuted = true;
            
            if (!File.Exists(_filePath))
            {
                _productCatalog = new Dictionary<string, Product>();
                SaveCatalog();
            }

            var json = File.ReadAllText(_filePath);
            _productCatalog = JsonConvert.DeserializeObject<Dictionary<string, Product>>(json, GetJsonSettings())
                   ?? new Dictionary<string, Product>();
        }

        private void SaveCatalog()
        {
            SaveCatalogExecuted = true;
            var json = JsonConvert.SerializeObject(_productCatalog, Formatting.Indented, GetJsonSettings());
            File.WriteAllText(_filePath, json);
        }

        private static JsonSerializerSettings GetJsonSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
        }
    }
}