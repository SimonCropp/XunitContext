namespace Xunit
{
    public class UIntCounter :
        Counter<uint>
    {
        protected override uint Convert(int i)
        {
            return (uint) i;
        }
    }
}