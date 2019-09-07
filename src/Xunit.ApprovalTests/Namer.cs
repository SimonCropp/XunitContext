using System.IO;
using System.Linq;
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
        return $".{additionalInformation}";
    }

    public string Name
    {
        get
        {
            var testCase = XunitLogging.Context.Test.TestCase;
            var arguments = testCase.TestMethodArguments;
            var name = $"{testCase.TestMethod.TestClass.Class.Name}.{testCase.TestMethod.Method.Name}";
            if (arguments == null || !arguments.Any())
            {
                return $"{name}{AdditionalInfo()}";
            }
            var suffix = string.Join("_", arguments);
            return $"{name}_{suffix}{AdditionalInfo()}";
        }
    }
}