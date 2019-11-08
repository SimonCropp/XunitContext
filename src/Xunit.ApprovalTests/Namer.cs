using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using Xunit.Abstractions;

class Namer:
    IApprovalNamer
{
    public string SourcePath
    {
        get
        {
            var context = XunitLogging.Context;

            var directory = Path.GetDirectoryName(context.SourceFile);
            if (TryGetSubdirectoryFromAttribute(context.Test.TestCase.TestMethod, out var subDirectory))
            {
                return Path.Combine(directory, subDirectory);
            }

            return directory;
        }
    }

    static string subDirAttribute = typeof(UseApprovalSubdirectoryAttribute).AssemblyQualifiedName;

    static bool TryGetSubdirectoryFromAttribute(ITestMethod method, [NotNullWhen(true)] out string? subDirectory)
    {
        var attribute = method.Method.GetCustomAttributes(subDirAttribute).SingleOrDefault();
        if (attribute != null)
        {
            subDirectory = attribute.GetNamedArgument<string>("Subdirectory");
            return true;
        }

        attribute = method.TestClass.Class.GetCustomAttributes(subDirAttribute).SingleOrDefault();
        if (attribute != null)
        {
            subDirectory = attribute.GetNamedArgument<string>("Subdirectory");
            return true;
        }

        subDirectory = null;
        return false;
    }

    static string? AdditionalInfo()
    {
        var additionalInformation = NamerFactory.AdditionalInformation;
        if (additionalInformation == null)
        {
            return additionalInformation;
        }
        return $"_{additionalInformation}";
    }

    public string Name
    {
        get => $"{XunitLogging.Context.UniqueTestName}{AdditionalInfo()}";
    }
}