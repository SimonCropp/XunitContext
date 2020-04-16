#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning restore CS3009 // Base type is not CLS-compliant
namespace Xunit
{
#pragma warning disable CS3009 // Base type is not CLS-compliant
    public class UIntCounter :
#pragma warning restore CS3009 // Base type is not CLS-compliant
        Counter<uint>
    {
        protected override uint Convert(int i)
        {
            return (uint) i;
        }
    }
}