namespace SupermarketReceipt;

public interface ICatalog
{
    int NumberOfProducts { get; }
    bool Contains(string productId);
    Product Product(string productId);
    void AddProduct(Product product);
    void RemoveProduct(string productId);

    void ApplySpecialDeal
    (
        SpecialDeal deal,
        string productId
    );

    void Clear();
}