using System.Net.Mail;

namespace SupermarketReceipt;

public class ReceiptMailer
{
    public void Send
    (
        Receipt receipt,
        string emailAddress
    )
    {
        var mailMessage = new MailMessage
            (
                "receipt@supermarket.com", 
                emailAddress, 
                "Supermarket Receipt", 
                Body(receipt)
            );
        
        SupermarketSmtpClient.Send(mailMessage);
    }

    private static string? Body
    (
        Receipt receipt
    )
    {
        var message = "Thank you for your purchase of " + receipt.NumberOfLineItems + " items!<br /><br />";
        
        message += "<table>"+ Environment.NewLine;
        
        message += "<tr>"+ Environment.NewLine;
        message += "<th>Description </th>"+ Environment.NewLine;
        message += "<th>Quantity</th>"+ Environment.NewLine;
        message += "<th>Discount</th>"+ Environment.NewLine;
        message += "<th>Unit Price</th>"+ Environment.NewLine;
        message += "<th>Billable Amount</th>"+ Environment.NewLine;
        message += "</tr>"+ Environment.NewLine;

        foreach (var lineItem in receipt.LineItems)
        {
            message += "<tr>"+ Environment.NewLine;
            message += "<td>" + lineItem.Description + "</td>"+ Environment.NewLine;
            message += "<td>" + lineItem.Quantity + "</td>"+ Environment.NewLine;
            message += "<td>" + lineItem.Discount + "%</td>"+ Environment.NewLine;
            message += "<td>$" + lineItem.UnitPrice + "</td>"+ Environment.NewLine;
            message += "<td>$" + lineItem.BillableAmount + "</td>"+ Environment.NewLine;
            message += "</tr>"+ Environment.NewLine;
        }
        
        message += "<tr>"+ Environment.NewLine;
        message += "<td></td>"+ Environment.NewLine;
        message += "<td></td>"+ Environment.NewLine;
        message += "<td></td>"+ Environment.NewLine;
        message += "<td>Total:</td>"+ Environment.NewLine;
        message += "<td>$" + receipt.TotalAmount + "</td>"+ Environment.NewLine;
        message += "</tr>"+ Environment.NewLine;
        
        message += "</table>"+ Environment.NewLine;

        return message;
    }

    private class SupermarketSmtpClient
    {
        public static void Send
        (
            MailMessage message
        )
        {
            var text = "<b>From</b>: " + message.From + Environment.NewLine + "<br />";
            text += "<b>To</b>: " + message.To + Environment.NewLine + "<br />";
            text += "<b>Subject</b>: " + message.Subject + Environment.NewLine + Environment.NewLine + "<br /><br />";
            text += message.Body;
            
            File.WriteAllText("ReceiptEmail.html", text);
        }
    }
}