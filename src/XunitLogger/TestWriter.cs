using System.IO;
using System.Text;
using System.Threading.Tasks;

class TestWriter : TextWriter
{
    public override Encoding Encoding { get; }

    public override void Write(char value)
    {
        XunitLogging.Write(value);
    }

    public override void Write(string value)
    {
        XunitLogging.Write(value);
    }

    public override void WriteLine()
    {
        XunitLogging.WriteLine();
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