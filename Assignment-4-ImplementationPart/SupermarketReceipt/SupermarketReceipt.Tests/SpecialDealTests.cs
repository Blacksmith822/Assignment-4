using System.Reflection;
using Newtonsoft.Json;

namespace SupermarketReceipt.Tests;

public class SpecialDealTests : DisposableTest
{
    [Fact]
    public void SpecialDeal_VerifyNameOfClass_ExpectClassNameIsSpecialDeal()
    {
        Assert.Equal("SpecialDeal", nameof(SpecialDeal)); 
    }
    
    [Fact]
    public void SpecialDeal_CannotBeInstantiated_ExpectClassIsAbstract()
    {
        Assert.True(typeof(SpecialDeal).IsAbstract);
    }
    
    [Fact]
    public void SpecialDeal_SerializableToJson_ExpectJsonObjectAttribute()
    {
        Assert.True(typeof(SpecialDeal).GetCustomAttributes(typeof(JsonObjectAttribute), inherit: true).Any());
    }

    [Fact]
    public void Discount_IsAAbstractMethod_ExpectAbstractDiscountMethodExists()
    {
        var methods = typeof(SpecialDeal).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.Contains(methods, m => m is {IsAbstract: true, Name: "Discount"});
    }

    [Fact]
    public void Discount_HasTwoInputParameters_ExpectTwoParametersExistForDiscountMethod()
    {
        var method = typeof(SpecialDeal).GetMethod("Discount", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        Assert.Equal(2, method?.GetParameters().Length);
    }
}