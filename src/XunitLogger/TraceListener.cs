class TraceListener : System.Diagnostics.TraceListener
{
    public override void Write(string value)
    {
        if (value != null)
        {
            XunitLogger.Write(value);
        }
    }

    public override void WriteLine(string value)
    {
        if (value == null)
        {
            XunitLogger.WriteLine();
        }
        else
        {
            XunitLogger.WriteLine(value);
        }
    }
}