namespace Xunit
{
    public class ULongCounter :
        Counter<ulong>
    {
        protected override ulong Convert(int i)
        {
            return (ulong) i;
        }
    }
}