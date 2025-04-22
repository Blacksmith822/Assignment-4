using System.Reflection;
using Newtonsoft.Json;

namespace SupermarketReceipt.Tests;

public class XForThePriceOfYSpecialDealTests : DisposableTest
{
    private readonly PropertyInfo? _quantityPropertyInfo;
    private readonly PropertyInfo? _forThePriceOfPropertyInfo;
    
    private int _discountQuantity;
    private int _forThePriceOf;
    private int _orderQuantity;
    private int _unitPrice;

    public XForThePriceOfYSpecialDealTests()
    {
        _quantityPropertyInfo = typeof(XForThePriceOfYSpecialDeal).GetProperty("Quantity");
        _forThePriceOfPropertyInfo = typeof(XForThePriceOfYSpecialDeal).GetProperty("ForThePriceOf");
    }
    
    [Fact]
    public void XForThePriceOfYSpecialDeal_VerifyNameOfClass_ExpectClassNameIsXForThePriceOfYSpecialDeal()
    {
        Assert.Equal("XForThePriceOfYSpecialDeal", nameof(XForThePriceOfYSpecialDeal)); 
    }
    
    [Fact]
    public void JsonProperties_SerializableToJson_ExpectTwoPropertiesWithJsonPropertyAttribute()
    {
        Assert.Equal
        (
            2,
            typeof(XForThePriceOfYSpecialDeal)
            .GetProperties()
            .Count(prop => prop.GetCustomAttributes(typeof(JsonPropertyAttribute), inherit: true).Any())
        );
    }
    
    [Fact]
    public void Quantity_SerializableToJson_ExpectJsonPropertyAttribute()
    {
        Assert.True(_quantityPropertyInfo?.GetCustomAttributes(typeof(JsonPropertyAttribute), true).Any());
    }
    
    [Fact]
    public void ForThePriceOf_SerializableToJson_ExpectJsonPropertyAttribute()
    {
        Assert.True(_forThePriceOfPropertyInfo?.GetCustomAttributes(typeof(JsonPropertyAttribute),true).Any());
    }
    
    [Fact]
    public void Quantity_AssignAValue_ExpectValue()
    {
        const int quantity = 2;
        const int forThePriceOf = 1;
        var deal = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf);
        Assert.Equal(quantity, deal.Quantity);
    }
    
    [Fact]
    public void ForThePriceOf_AssignAValue_ExpectValue()
    {
        const int quantity = 2;
        const int forThePriceOf = 1;
        var deal = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf);
        Assert.Equal(forThePriceOf, deal.ForThePriceOf);
    }
    
    [Fact]
    public void Discount_TwoForThePriceOfOne_ExpectFiftyPercentDiscount()
    {
        const int quantity = 2;
        const int forThePriceOf = 1;
        var discount = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf).Discount(4, 10);
        Assert.Equal(50m, discount);
    }
    
    [Fact]
    public void Discount_FourForThePriceOfThree_ExpectTwentyFivePercentDiscount()
    {
        const int quantity = 4;
        const int forThePriceOf = 3;
        var discount = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf).Discount(4, 10);
        Assert.Equal(25m, discount);
    }
    
    [Fact]
    public void Discount_OneForThePriceOfOne_ExpectZeroPercentDiscount()
    {
        const int quantity = 1;
        const int forThePriceOf = 1;
        var discount = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf).Discount(4, 10);
        Assert.Equal(0m, discount);
    }
    
    [Fact]
    public void Discount_TenForThePriceOfOne_ExpectNinetyPercentDiscount()
    {
        const int quantity = 10;
        const int forThePriceOf = 1;
        var discount = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf).Discount(20, 10);
        Assert.Equal(90m, discount);
    }
    
    [Fact]
    public void Discount_TenForThePriceOfZero_ExpectOneHundredPercentDiscount()
    {
        const int quantity = 10;
        const int forThePriceOf = 0;
        var discount = new XForThePriceOfYSpecialDeal(quantity, forThePriceOf).Discount(20, 10);
        Assert.Equal(100m, discount);
    }
    
    [Fact]
    public void Discount_TenForThePriceOfOneButLessThanTenPurchased_ExpectZeroPercentDiscount()
    {
        _discountQuantity = 10;
        _forThePriceOf = 1;
        _orderQuantity = 9;
        _unitPrice = 10;
        var actualDiscount = new XForThePriceOfYSpecialDeal(_discountQuantity, _forThePriceOf).Discount(_orderQuantity, _unitPrice);
        var expectedDiscount = ExpectedDiscount(_discountQuantity, _forThePriceOf, _orderQuantity, _unitPrice);
        Assert.Equal(expectedDiscount, actualDiscount);
    }
    
    [Fact]
    public void Discount_OneForThePriceOfTen_ExpectNineHundredPercentIncrease()
    {
        _discountQuantity = 1;
        _forThePriceOf = 10;
        _orderQuantity = 4;
        _unitPrice = 10;
        var actualDiscount = new XForThePriceOfYSpecialDeal(_discountQuantity, _forThePriceOf).Discount(_orderQuantity, _unitPrice);
        var expectedDiscount = ExpectedDiscount(_discountQuantity, _forThePriceOf, _orderQuantity, _unitPrice);
        Assert.Equal(expectedDiscount, actualDiscount);
    }

    [Fact]
    public void XForThePriceOfYSpecialDeal_SerializedToJson_ExpectJson()
    {
        var actualJson = JsonConvert.SerializeObject(new XForThePriceOfYSpecialDeal(3, 1));
        const string expectedJson = "{\"Quantity\":3.0,\"ForThePriceOf\":1.0}";
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void XForThePriceOfYSpecialDeal_DeserializedFromJson_ExpectSpecialDealObject()
    {
        const string json = "{\"Quantity\":3.0,\"ForThePriceOf\":1.0}";
        var deal = JsonConvert.DeserializeObject<XForThePriceOfYSpecialDeal>(json);
        Assert.IsType<XForThePriceOfYSpecialDeal>(deal);
        Assert.Equal(3, deal.Quantity);
        Assert.Equal(1, deal.ForThePriceOf);
    }

    [Fact]
    public void XForThePriceOfYSpecialDeal_IsASpecialDeal_ExpectSpecialDealIsBaseType()
    {
        Assert.IsAssignableFrom<SpecialDeal>(new XForThePriceOfYSpecialDeal(2, 1));
    }

    private static decimal ExpectedDiscount(decimal discountQuantity, decimal forThePriceOf, int orderQuantity, decimal unitPrice)
    {
        var numberNotToDiscount = orderQuantity % discountQuantity;
        var numberToDiscount = Math.Floor(orderQuantity / discountQuantity);
        var discountedAmount = numberToDiscount * unitPrice * forThePriceOf;
        var remainingRegularAmount = numberNotToDiscount * unitPrice;
        var totalDiscountedAmount = discountedAmount + remainingRegularAmount;
        var totalRegularAmount = orderQuantity * unitPrice;

        return ((totalRegularAmount - totalDiscountedAmount) / totalRegularAmount) * 100;
    }
}