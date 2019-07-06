using Xunit;
using XunitLogger;

public class CountersSample
{
    [Fact]
    public void Usage()
    {
        #region NonTestContextUsage
        var current = Counters.CurrentGuid;

        var next = Counters.NextGuid();

        var counter = new GuidCounter();
        var localCurrent = counter.Current;
        var localNext = counter.Next();
        #endregion
    }
}