using System.Diagnostics;

class TraceListener : System.Diagnostics.TraceListener
{
    public override void Write(string message)
    {
        XunitLogger.Write(message);
    }

    public override void WriteLine(string message)
    {
        XunitLogger.WriteLine(message);
    }
}