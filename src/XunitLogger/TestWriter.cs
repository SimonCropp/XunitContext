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
        XunitLogger.WriteLine();
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

    public override Task WriteAsync(char value)
    {
        Write(value);
        return Task.CompletedTask;
    }

    public override Task WriteAsync(string value)
    {
        Write(value);
        return Task.CompletedTask;
    }

    public override Task WriteLineAsync(string value)
    {
        WriteLine(value);
        return Task.CompletedTask;
    }
}