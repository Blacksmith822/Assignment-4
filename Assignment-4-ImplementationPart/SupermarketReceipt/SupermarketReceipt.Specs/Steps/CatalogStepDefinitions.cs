using Xunit;

namespace SupermarketReceipt.Specs.Steps;

[Binding]
public class CatalogStepDefinitions
{
    private Catalog _catalog;
    private Exception _exception;

    [Given(@"an empty catalog")]
    public void GivenAnEmptyCatalog()
    {
        _catalog = new Catalog();
        _catalog.Clear();
    }

    [When(@"these products are added to the catalog:")]
    public void WhenTheseProductsAreAddedToTheCatalog
    (
        Table table
    )
    {
        try
        {
            foreach (var row in table.Rows)
            {
                _catalog.AddProduct
                (
                    new Product(row["Product"], Convert.ToDecimal(row["Price"]))
                );
            }
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [When(@"these products are removed from the catalog:")]
    public void WhenTheseProductsAreRemovedFromTheCatalog
    (
        Table table
    )
    {
        try
        {
            foreach (var row in table.Rows)
            {
                _catalog.RemoveProduct(row["Product"]);
            }
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Then(@"the catalog should have (.*) products")]
    public void ThenTheCatalogShouldHaveProducts
    (
        int expectNumberOfProducts
    )
    {
        Assert.Equal(expectNumberOfProducts, _catalog.NumberOfProducts);
    }

    [Then(@"contain (.*) at (.*)")]
    public void ThenContainItemAt
    (
        string productId,
        decimal price
    )
    {
        var product = _catalog.Product(productId);
        
        Assert.Equal(productId, product.ProductId);
        Assert.Equal(price, product.UnitPrice);
    }

    [Then(@"there should be an exception saying ""(.*)""")]
    public void ThenThereShouldBeAnExceptionSaying
    (
        string expectedExceptionMessage
    )
    {
        Assert.Equal(expectedExceptionMessage, _exception.Message);
    }

    [Then(@"there should be no exception")]
    public void ThenThereShouldBeNoException()
    {
        Assert.Null(_exception);
    }

    [When(@"a simple (.*) percent discount is applied to (.*)")]
    public void WhenASimplePercentDiscountIsAppliedToItem(
        decimal percent,
        string productId
    )
    {
        var deal = new PercentDiscountSpecialDeal(percent);
        
        _catalog.ApplySpecialDeal(deal, productId);
    }

    [Then(@"contain (.*) with (.*) percent discount")]
    public void ThenContainItemWithPercentDiscount
    (
        string productId,
        int expectedPercentDiscount
    )
    {
        var product = _catalog.Product(productId);
        
        Assert.Equal(expectedPercentDiscount,product.Deal!.Discount(1, product.UnitPrice));
    }

    [When(@"a (.*) for the price of (.*) deal is applied to (.*)")]
    public void WhenAForThePriceOfDealIsAppliedToMilk
    (
        decimal manyQuantity,
        decimal fewQuantity,
        string productId
    )
    {
        var deal = new XForThePriceOfYSpecialDeal(manyQuantity, fewQuantity);
        
        _catalog.ApplySpecialDeal(deal, productId);
    }

    [Then(@"contain (.*) with a (.*) for the price of (.*) deal")]
    public void ThenContainItemWithAManyForThePriceOfAFewDeal
    (
        string productId,
        decimal manyQuantity,
        decimal fewQuantity
    )
    {
        var deal = (XForThePriceOfYSpecialDeal)_catalog.Product(productId).Deal!;
        
        Assert.Equal(manyQuantity, deal.Quantity);
        Assert.Equal(fewQuantity, deal.ForThePriceOf);
    }
}