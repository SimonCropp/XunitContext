using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ApprovalTests.Core;
using ApprovalTests.Namers;

class Namer:
    IApprovalNamer
{
    public string SourcePath
    {
        get
        {
            return Path.GetDirectoryName(XunitLogging.Context.SourceFilePath);
        }
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
                builder.Append($"{parameterInfo.Name}={argument}_");
            }

            builder.Length = builder.Length - 1;

            var formattableString = $"{name}_{builder}{AdditionalInfo()}";
            return formattableString;
        }
    }
}