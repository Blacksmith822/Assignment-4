namespace SupermarketReceipt.Tests;

public class DisposableTest : IDisposable
{
    public void Dispose()
    {
        if(File.Exists("catalog.txt"))
            File.Delete("catalog.txt");
        if(File.Exists("ReceiptEmail.html"))
            File.Delete("ReceiptEmail.html");
    }
}