using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using XunitLogger;

class Namer:
    IApprovalNamer
{
    public static Namer Instance = new Namer();

    public string SourcePath
    {
        get
        {
            var context = XunitLogging.Context;
            var directory = Path.GetDirectoryName(context.SourceFile);
            if (TryGetSubdirectoryFromAttribute(context, out var subDirectory))
            {
                return Path.Combine(directory, subDirectory);
            }

            return directory;
        }
    }

    static string subDirAttribute = typeof(UseApprovalSubdirectoryAttribute).AssemblyQualifiedName;

    static bool TryGetSubdirectoryFromAttribute(Context context, [NotNullWhen(true)] out string? subDirectory)
    {
        var method = context.MethodInfo;
        var attribute = (UseApprovalSubdirectoryAttribute)method.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute), true);
        if (attribute == null)
        {
            attribute = (UseApprovalSubdirectoryAttribute)method.DeclaringType.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute), true);
        }
        if (attribute == null)
        {
            attribute = (UseApprovalSubdirectoryAttribute)method.DeclaringType.Assembly.GetCustomAttribute(typeof(UseApprovalSubdirectoryAttribute));
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
        get => $"{XunitLogging.Context.UniqueTestName}{AdditionalInfo()}";
    }
}