using System;
using System.IO;
using System.Linq;

static class SolutionDirectoryFinder
{
    public static string Find(string testDirectory)
    {
        if (!TryFind(testDirectory, out var solutionDirectory))
        {
            throw new Exception("Could not find solution directory");
        }

        return solutionDirectory;
    }

    public static bool TryFind(string testDirectory, out string path)
    {
        var currentDirectory = testDirectory;
        do
        {
            if (Directory.GetFiles(currentDirectory, "*.sln").Any())
            {
                path = currentDirectory;
                return true;
            }

            var parent = Directory.GetParent(currentDirectory);
            if (parent == null)
            {
                path = string.Empty;
                return false;
            }

            currentDirectory = parent.FullName;
        } while (true);
    }
}