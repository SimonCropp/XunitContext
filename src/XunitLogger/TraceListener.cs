class TraceListener : System.Diagnostics.TraceListener
{
    public override void Write(string value)
    {
        if (value != null)
        {
            XunitLogging.Write(value);
        }
    }

    public override void WriteLine(string value)
    {
        if (value == null)
        {
            XunitLogging.WriteLine();
        }
        else
        {
            XunitLogging.WriteLine(value);
        }
    }

    public override bool IsThreadSafe => true;
}