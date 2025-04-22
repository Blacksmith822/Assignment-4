using System.Reflection;
using Newtonsoft.Json;

namespace SupermarketReceipt.Tests;

public class ProductTests : DisposableTest
{
    [Fact]
    public void Product_VerifyNameOfClass_ExpectClassNameIsProduct()
    {
        Assert.Equal("Product", nameof(Product)); 
    }

    [Fact]
    public void ApplySpecialDeal_VerifyAccessor_ExpectMethodIsPublicWithSpecificName()
    {
        var methods = typeof(Product).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.Contains(methods, m => m is {IsPublic: true, Name: "ApplySpecialDeal"});
    }
    
    [Fact]
    public void Product_VerifyNumberOfPublicProperties_ExpectThreePublicProperties()
    {
        var type = typeof(Product); 
        var publicProperties = type.GetProperties();
        var propertyCount = publicProperties.Length;
        Assert.Equal(3, propertyCount);
    }
    
    [Fact]
    public void Product_VerifyAllPublicProperties_ExpectEachToHavePublicGetterAndPrivateSetter()
    {
        var properties = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            Assert.True(property.GetMethod?.IsPublic, $"Property '{property.Name}' does not have a public getter.");
            Assert.True(property.SetMethod == null || property.SetMethod.IsPrivate, $"Property '{property.Name}' does not have a private setter.");
        }
    }
    
    [Fact]
    public void Product_VerifyConstructorSetsProductId_ExpectProductIdSetToValueSuppliedToConstructor()
    {
        const string productId = "ITEM_1234";
        var product = new Product(productId, 1);
        Assert.Equal(productId, product.ProductId);
    }
    
    [Fact]
    public void Product_VerifyConstructorSetsUnitPrice_ExpectUnitPriceSetToValueSuppliedToConstructor()
    {
        const decimal unitPrice = 111.22m;
        var product = new Product("", unitPrice);
        Assert.Equal(unitPrice, product.UnitPrice);
    }
    
    [Fact]
    public void Product_VerifyConstructorSetsDeal_ExpectDealSetToValueSuppliedToConstructor()
    {
        var deal = new DummySpecialDeal();
        var product = new Product("", 0, deal);
        Assert.Equal(deal, product.Deal);
    }
    
    [Fact]
    public void ApplySpecialDeal_ChangeTheSpecialDeal_ExpectDealSetToValueSuppliedToApplySpecialDealMethod()
    {
        var deal = new DummySpecialDeal();
        var product = new Product("", 0, deal);
        product.ApplySpecialDeal(new AnotherDummyDeal());
        Assert.IsType<AnotherDummyDeal>(product.Deal);
    }

    [Fact]
    public void Product_SerializedToJson_ExpectJson()
    {
        var actualJson = JsonConvert.SerializeObject(new Product("", 0));
        const string expectedJson = "{\"ProductId\":\"\",\"UnitPrice\":0.0,\"Deal\":null}";
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void Product_DeserializedFromJson_ExpectSpecialDealObject()
    {
        const string json = "{\"ProductId\":\"PROD\",\"UnitPrice\":10.0,\"Deal\":null}";
        var product = JsonConvert.DeserializeObject<Product>(json);
        Assert.IsType<Product>(product);
        Assert.Equal("PROD", product.ProductId);
        Assert.Equal(10m, product.UnitPrice);
        Assert.Null(product.Deal);
    }
    
    private class DummySpecialDeal : SpecialDeal
    {
        public override decimal Discount
        (
            int quantity,
            decimal unitPrice
        )
        {
            return 0;
        }
    }

    private class AnotherDummyDeal : DummySpecialDeal
    {
    }
}