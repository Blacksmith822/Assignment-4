namespace SupermarketReceipt.Tests;

public class ReceiptMailerTests : DisposableTest
{
    [Fact]
    public void ReceiptMailer_VerifyNameOfClass_ExpectClassNameIsReceiptMailer()
    {
        Assert.Equal("ReceiptMailer", nameof(ReceiptMailer)); 
    }
    
    [Fact]
    public void Send_SendReceiptToEmailAddress_ExpectEmailSent()
    {
        var product = new Product("TEST", 20);
        var receipt = new Receipt(new List<Product> {product});
        const string emailAddress = "customer@mail.com";
        new ReceiptMailer().Send(receipt, emailAddress);
        const string expectedText = "<b>From</b>: receipt@supermarket.com\r\n<br /><b>To</b>: customer@mail.com\r\n<br /><b>Subject</b>: Supermarket Receipt\r\n\r\n<br /><br />Thank you for your purchase of 1 items!<br /><br /><table>\r\n<tr>\r\n<th>Description </th>\r\n<th>Quantity</th>\r\n<th>Discount</th>\r\n<th>Unit Price</th>\r\n<th>Billable Amount</th>\r\n</tr>\r\n<tr>\r\n<td>TEST</td>\r\n<td>1</td>\r\n<td>0%</td>\r\n<td>$20</td>\r\n<td>$20</td>\r\n</tr>\r\n<tr>\r\n<td></td>\r\n<td></td>\r\n<td></td>\r\n<td>Total:</td>\r\n<td>$20</td>\r\n</tr>\r\n</table>\r\n";
        var text = File.ReadAllText("ReceiptEmail.html");
        Assert.Equal(expectedText, text);
    }
}