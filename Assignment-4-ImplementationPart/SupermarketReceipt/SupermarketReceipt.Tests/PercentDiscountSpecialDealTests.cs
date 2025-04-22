using System.Reflection;
using Newtonsoft.Json;

namespace SupermarketReceipt.Tests;

public class PercentDiscountSpecialDealTests : DisposableTest
{
    private readonly FieldInfo? _percentFieldInfo;

    public PercentDiscountSpecialDealTests()
    {
        _percentFieldInfo = typeof(PercentDiscountSpecialDeal)
            .GetField
            (
                "_percent", 
                BindingFlags.NonPublic | BindingFlags.Instance
            );
    }
    
    [Fact]
    public void PercentDiscountSpecialDeal_VerifyNameOfClass_ExpectClassNameIsPercentDiscountSpecialDeall()
    {
        Assert.Equal("PercentDiscountSpecialDeal", nameof(PercentDiscountSpecialDeal)); 
    }
    
    [Fact]
    public void JsonFields_SerializableToJson_ExpectOneFieldWithJsonPropertyAttribute()
    {
        Assert.Equal
        (
            1,
            typeof(PercentDiscountSpecialDeal)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(field => field.GetCustomAttributes(typeof(JsonPropertyAttribute), inherit: true).Any())
        );
    }
    
    [Fact]
    public void _percent_SerializableToJson_ExpectJsonPropertyAttribute()
    {
        Assert.True(_percentFieldInfo?.GetCustomAttributes(typeof(JsonPropertyAttribute), true).Any());
    }
    
    [Fact]
    public void Discount_TenPercentDiscountRegardlessOfOrderQuantityOrUnitPrice_ExpectTenPercentDiscount()
    {
        const decimal percentDiscount = 10m;
        var discount = new PercentDiscountSpecialDeal(percentDiscount).Discount(2, 13.50m);
        Assert.Equal(percentDiscount, discount);
    }
    
    [Fact]
    public void Discount_FiftyPercentDiscountRegardlessOfOrderQuantityOrUnitPrice_ExpectFiftyPercentDiscount()
    {
        const decimal percentDiscount = 50m;
        var discount = new PercentDiscountSpecialDeal(percentDiscount).Discount(200, 130.50m);
        Assert.Equal(percentDiscount, discount);
    }
    
    [Fact]
    public void Discount_NinetyPercentDiscountRegardlessOfOrderQuantityOrUnitPrice_ExpectNinetyPercentDiscount()
    {
        const decimal percentDiscount = 90m;
        var discount = new PercentDiscountSpecialDeal(percentDiscount).Discount(200, 130.50m);
        Assert.Equal(percentDiscount, discount);
    }

    [Fact]
    public void PercentDiscountSpecialDeal_SerializedToJson_ExpectJson()
    {
        var actualJson = JsonConvert.SerializeObject(new PercentDiscountSpecialDeal(50));
        const string expectedJson = "{\"_percent\":50.0}";
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void PercentDiscountSpecialDeal_DeserializedFromJson_ExpectSpecialDealObject()
    {
        const string json = "{\"_percent\":50.0}";
        var deal = JsonConvert.DeserializeObject<PercentDiscountSpecialDeal>(json);
        var discount = deal?.Discount(200, 130.50m);
        Assert.IsType<PercentDiscountSpecialDeal>(deal);
        Assert.Equal(50m, discount);
    }

    [Fact]
    public void PercentDiscountSpecialDeal_IsASpecialDeal_ExpectSpecialDealIsBaseType()
    {
        Assert.IsAssignableFrom<SpecialDeal>(new PercentDiscountSpecialDeal(1));
    }
}