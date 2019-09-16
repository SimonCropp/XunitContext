using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            var directory = Path.GetDirectoryName(context.SourceFilePath);
            if (TryGetSubdirectoryFromAttribute(context.Test.TestCase.TestMethod, out var subDirectory))
            {
                return Path.Combine(directory, subDirectory);
            }

            return directory;
        }
    }
    
    static string subDirAttribute = typeof(UseApprovalSubdirectoryAttribute).AssemblyQualifiedName;

    static bool TryGetSubdirectoryFromAttribute(ITestMethod method, out string subDirectory)
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

    string AdditionalInfo()
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
        get
        {
            var testCase = XunitLogging.Context.Test.TestCase;
            var arguments = testCase.TestMethodArguments;
            var method = testCase.TestMethod;
            var name = $"{method.TestClass.Class.Name}.{method.Method.Name}";
            if (arguments == null || !arguments.Any())
            {
                return $"{name}{AdditionalInfo()}";
            }

            var builder = new StringBuilder();
            var parameterInfos = method.Method.GetParameters().ToList();
            for (var index = 0; index < parameterInfos.Count; index++)
            {
                var parameterInfo = parameterInfos[index];
                var argument = arguments[index];
                if (argument == null)
                {
                    builder.Append($"{parameterInfo.Name}=null_");
                    continue;
                }
                builder.Append($"{parameterInfo.Name}={argument}_");
            }

            builder.Length -= 1;

            return $"{name}_{builder}{AdditionalInfo()}";
        }
    }
}