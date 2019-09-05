using System.IO;
using System.Linq;
using ApprovalTests.Core;

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

    public string Name
    {
        get
        {
            var testCase = XunitLogging.Context.Test.TestCase;
            var name = $"{testCase.TestMethod.TestClass.Class.Name}_{testCase.TestMethod.Method.Name}";
            if (!testCase.TestMethodArguments.Any())
            {
                return name;
            }
            var suffix = string.Join("_", testCase.TestMethodArguments);
            return $"{name}_{suffix}";
        }
    }
}