namespace Xunit
{
    public class IntCounter :
        Counter<int>
    {
        protected override int Convert(int i)
        {
            return i;
        }
    }
}