using Xunit;

namespace SupermarketReceipt.Specs.Steps;

[Binding]
public sealed class ShoppingCartStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private Catalog _catalog;
    private ShoppingCart _shoppingCart;
    private Receipt _receipt;
    private string _emailAddress;
    private Exception _exception;

    public ShoppingCartStepDefinitions
    (
        ScenarioContext scenarioContext
    )
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the catalog contains:")]
    public void GivenTheCatalogContains
    (
        Table table
    )
    {
        _catalog = new Catalog();
        _catalog.Clear();

        foreach (var row in table.Rows)
        {
            _catalog.AddProduct
                (
                    new Product(row["Product"], Convert.ToDecimal(row["Price"]))
                );
        }
    }

    [Given(@"an empty shopping cart")]
    public void GivenAnEmptyShoppingCart()
    {
        try
        {
            _shoppingCart = new ShoppingCart(_catalog);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [Given(@"(.*) has a (.*) percent discount")]
    public void GivenItemHasAPercentDiscount
    (
        string productId,
        int discountPercent
    )
    {
        try
        {
            _catalog.ApplySpecialDeal(new PercentDiscountSpecialDeal(discountPercent), productId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [When(@"(.*) is added to the cart")]
    public void WhenItemIsAddedToTheCart
    (
        string productId
    )
    {
        try
        {
            _shoppingCart.AddItemByProductId(productId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [When(@"(.*) is removed from the cart")]
    public void WhenItemIsRemovedFromTheCart
    (
        string productId
    )
    {
        try
        {
            _shoppingCart.RemoveItemByProductId(productId);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [When(@"a receipt is generated")]
    public void WhenAReceiptIsGenerated()
    {
        try
        {
            _receipt = _shoppingCart.Receipt();
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }

    [When(@"the receipt is emailed to (.*)")]
    public void WhenTheReceiptIsEmailedTo
    (
        string emailAddress
    )
    {
        try
        {
            _emailAddress = emailAddress;
            new ReceiptMailer().Send(_receipt, emailAddress);
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }
    
    [Then(@"there should should be (.*) items in the cart")]
    public void ThenThereShouldShouldBeItemsInTheCart
    (
        int expectNumberOfItems
    )
    {
        Assert.Equal(expectNumberOfItems, _shoppingCart.NumberOfItems);
    }

    [Then(@"the receipt's total amount should be (.*)")]
    public void ThenTheReceiptsTotalAmountShouldBe
    (
        decimal expectedTotalAmount
    )
    {
        Assert.Equal(expectedTotalAmount, _receipt.TotalAmount);
    }

    [Then(@"the receipt should have:")]
    public void ThenTheReceiptShouldHave
    (
        Table table
    )
    {
        foreach (var row in table.Rows)
        {
            var receiptLine = _receipt.LineItem(row["Product"]);
            
            Assert.Equal(row["Product"], receiptLine.Description);
            Assert.Equal(Convert.ToInt32(row["Quantity"]), receiptLine.Quantity);
            Assert.Equal(Convert.ToDecimal(row["Amount"]), receiptLine.BillableAmount);
        }
    }

    [Then(@"there should been an exception saying ""(.*)""")]
    public void ThenThereShouldBeenAnExceptionSaying
    (
        string expectedExceptionMessage
    )
    {
        Assert.Equal(expectedExceptionMessage, _exception.Message);
    }

    [Then(@"there should not be an exception")]
    public void ThenThereShouldNotBeAnException()
    {
        Assert.Null(_exception);
    }

    [Then(@"the receipt should have been emailed")]
    public void ThenTheReceiptShouldHaveBeenEmailed()
    {
        try
        {
            var emailText = File.ReadAllText("ReceiptEmail.html");
            
            Assert.Contains(_emailAddress, emailText);

            foreach (var lineItem in _receipt.LineItems)
            {
                Assert.Contains(lineItem.Description, emailText);
            }
        }
        catch (Exception e)
        {
            _exception = e;
        }
    }
}