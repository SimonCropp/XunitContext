namespace Xunit
{
    public class LongCounter :
        Counter<long>
    {
        protected override long Convert(int i)
        {
            return i;
        }
    }
}