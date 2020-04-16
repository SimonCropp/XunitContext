#pragma warning disable CS3002 // Return type is not CLS-compliant
namespace Xunit
{
#pragma warning disable CS3009 // Base type is not CLS-compliant
    public class ULongCounter :
#pragma warning restore CS3009 // Base type is not CLS-compliant
        Counter<ulong>
    {
        protected override ulong Convert(int i)
        {
            return (ulong) i;
        }
    }
}