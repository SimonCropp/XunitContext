using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

class TestWriter :
    TextWriter
{
    public override Encoding Encoding { get; } = Encoding.UTF8;

    public override void Write(char value)
    {
        XunitContext.Write(value);
    }

    public override void Write(string value)
    {
        XunitContext.Write(value);
    }

    public override void WriteLine()
    {
        XunitContext.WriteLine();
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