using System.Diagnostics;

static class TestFileFinder
{
    public static string WalkStackTraceForFileWithConstructor(StackTrace trace)
    {
        var stackFrame = trace.GetFrame(0);
        var index = 0;
        while (true)
        {
            index++;
            var nextFrame = trace.GetFrame(index);
            var nextMethod = nextFrame.GetMethod();
            if (!nextMethod.IsConstructor)
            {
                return stackFrame.GetFileName();
            }

            stackFrame = nextFrame;
        }
    }
}