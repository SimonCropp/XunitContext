using Xunit;

class TraceListener : System.Diagnostics.TraceListener
{
    public override void Write(string value)
    {
        if (value != null)
        {
            XunitContext.Write(value);
        }
    }

    public override void WriteLine(string value)
    {
        if (value == null)
        {
            XunitContext.WriteLine();
        }
        else
        {
            XunitContext.WriteLine(value);
        }
    }

    public override bool IsThreadSafe => true;
}