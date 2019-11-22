using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using Xunit;

class Namer:
    IApprovalNamer
{
    public static Namer Instance = new Namer();

    public string SourcePath
    {
        get
        {
            var context = XunitContext.Context;
            var directory = Path.GetDirectoryName(context.SourceFile);
            if (TryGetSubdirectoryFromAttribute(context, out var subDirectory))
            {
                return Path.Combine(directory, subDirectory);
            }

            return directory;
        }
    }

    static bool TryGetSubdirectoryFromAttribute(Context context, [NotNullWhen(true)] out string? subDirectory)
    {
        var attribute = (UseApprovalSubdirectoryAttribute)context.MethodInfo.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute), true);
        if (attribute == null)
        {
            attribute = (UseApprovalSubdirectoryAttribute)context.TestType.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute), true);
        }
        if (attribute == null)
        {
            attribute = (UseApprovalSubdirectoryAttribute)context.TestType.Assembly.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute));
        }
        if (attribute != null)
        {
            subDirectory = attribute.Subdirectory;
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
        get => $"{XunitContext.Context.UniqueTestName}{AdditionalInfo()}";
    }
}