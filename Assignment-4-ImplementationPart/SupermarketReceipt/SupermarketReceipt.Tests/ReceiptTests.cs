using System.Reflection;

namespace SupermarketReceipt.Tests;

public class ReceiptTests : DisposableTest
{
    [Fact]
    public void Receipt_VerifyNameOfClass_ExpectClassNameIsReceipt()
    {
        Assert.Equal("Receipt", nameof(Receipt)); 
    }
    
    [Fact]
    public void Constructor_VerifyParameter_ExpectTakesOneParameter()
    {
        var constructors = typeof(Receipt).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);
        Assert.NotNull(constructor);
        Assert.Single(constructors);
        Assert.Equal(typeof(IEnumerable<Product>), constructor.GetParameters().First().ParameterType);
    }
    
    [Fact]
    public void Receipt_OneProduct_ExpectOneReceiptLine()
    {
        var product = new Product("TEST", 1);
        var receipt = new Receipt(new List<Product> {product});
        Assert.Equal(1, receipt.NumberOfLineItems);
        Assert.Single(receipt.LineItems);
        Assert.Equal(1, receipt.TotalAmount);
        
        Assert.Equal("TEST", receipt.LineItems.First().Description);
        Assert.Equal(1, receipt.LineItems.First().Quantity);
        Assert.Equal(1, receipt.LineItems.First().UnitPrice);
        Assert.Equal(0, receipt.LineItems.First().Discount);
    }
    
    [Fact]
    public void Receipt_TwoOfSameProduct_ExpectOneReceiptLine()
    {
        var product = new Product("TEST", 10);
        var receipt = new Receipt(new List<Product> {product, product});
        Assert.Equal(1, receipt.NumberOfLineItems);
        Assert.Equal(20, receipt.TotalAmount);
        
        Assert.Equal("TEST", receipt.LineItems.First().Description);
        Assert.Equal(2, receipt.LineItems.First().Quantity);
        Assert.Equal(10, receipt.LineItems.First().UnitPrice);
        Assert.Equal(0, receipt.LineItems.First().Discount);
    }
    
    [Fact]
    public void Receipt_TwoOfSameProductOneOfAnotherProduct_ExpectTwoReceiptLines()
    {
        var product1 = new Product("TEST", 10);
        var product2 = new Product("1234", 30);
        var receipt = new Receipt(new List<Product> {product1, product1, product2});
        Assert.Equal(2, receipt.NumberOfLineItems);
        Assert.Equal(50, receipt.TotalAmount);
        
        Assert.Equal("TEST", receipt.LineItems.ElementAt(0).Description);
        Assert.Equal(2, receipt.LineItems.ElementAt(0).Quantity);
        Assert.Equal(10, receipt.LineItems.ElementAt(0).UnitPrice);
        Assert.Equal(0, receipt.LineItems.ElementAt(0).Discount);
        
        Assert.Equal("1234", receipt.LineItems.ElementAt(1).Description);
        Assert.Equal(1, receipt.LineItems.ElementAt(1).Quantity);
        Assert.Equal(30, receipt.LineItems.ElementAt(1).UnitPrice);
        Assert.Equal(0, receipt.LineItems.ElementAt(1).Discount);
    }
    
    [Fact]
    public void Receipt_OneProductWithSpecialDeal_ExpectOneReceiptLineWithDiscount()
    {
        var product = new Product("TEST", 10, new MockFiftyPercentOffDeal());
        var receipt = new Receipt(new List<Product> {product});
        Assert.Equal(1, receipt.NumberOfLineItems);
        Assert.Single(receipt.LineItems);
        Assert.Equal(5, receipt.TotalAmount);
        
        Assert.Equal("TEST", receipt.LineItems.First().Description);
        Assert.Equal(1, receipt.LineItems.First().Quantity);
        Assert.Equal(10, receipt.LineItems.First().UnitPrice);
        Assert.Equal(50, receipt.LineItems.First().Discount);
    }
    
    [Fact]
    public void LineItem_RequestLineItemForAProductId_ExpectLineItem()
    {
        const string productId = "TEST";
        const decimal unitPrice = 10;
        var product = new Product(productId, unitPrice, new MockFiftyPercentOffDeal());
        var receipt = new Receipt(new List<Product> {product});
        var line = receipt.LineItem(productId);
        Assert.NotNull(line);
        Assert.Equal(productId, line.Description);
        Assert.Equal(1, line.Quantity);
        Assert.Equal(unitPrice, line.UnitPrice);
        Assert.Equal(50, line.Discount);
    }
    
    [Fact]
    public void Receipt_RequestReceiptForZeroProducts_ExpectException()
    {
        var exception = Assert.Throws<ApplicationException>(() => new Receipt(new List<Product>()));
        Assert.Equal("Cannot generate a receipt without items.", exception.Message);
    }
    
    [Fact]
    public void ReceiptLineItem_DefinedInReceiptClassFile_ExpectDefinitionInSourceCodeFile()
    {
        const string filePath = "../../../../SupermarketReceipt/Receipt.cs";
        Assert.True(File.Exists(filePath), $"The file '{filePath}' does not exist.");

        var sourceCode = File.ReadAllText(filePath);
        
        Assert.Contains("public class ReceiptLineItem", sourceCode);
    }
    
    [Fact]
    public void Price_DefinedInReceiptClassFile_ExpectDefinitionInSourceCodeFile()
    {
        const string filePath = "../../../../SupermarketReceipt/Receipt.cs";
        Assert.True(File.Exists(filePath), $"The file '{filePath}' does not exist.");

        var sourceCode = File.ReadAllText(filePath);
        
        Assert.Contains("private class Price", sourceCode);
    }

    private class MockFiftyPercentOffDeal : SpecialDeal
    {
        public override decimal Discount
        (
            int quantity,
            decimal unitPrice
        )
        {
            return 50;
        }
    }
}