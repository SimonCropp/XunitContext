using System.IO;
using System.Text;
using System.Threading.Tasks;

class TestWriter : TextWriter
{
    public override Encoding Encoding { get; }

    public override void Write(char value)
    {
        XunitLogger.Write(value);
    }

    public override void Write(string value)
    {
        XunitLogger.Write(value);
    }

    public override void WriteLine()
    {
        XunitLogger.WriteLine(null);
    }

    public override void WriteLine(string value)
    {
        XunitLogger.WriteLine(value);
    }

    public override Task WriteAsync(char value)
    {
        Write(value);
        return Task.CompletedTask;
    }

    public override Task WriteAsync(string value)
    {
        XunitLogger.Write(value);
        return Task.CompletedTask;
    }

    public override Task WriteLineAsync(string value)
    {
        XunitLogger.WriteLine(value);
        return Task.CompletedTask;
    }
}