using Xunit.Abstractions;

namespace Xunit
{
    public class Parameter
    {
        public IParameterInfo Info { get; }
        public object Value { get; }

        public Parameter(IParameterInfo info, object value)
        {
            Guard.AgainstNull(info, nameof(info));
            Info = info;
            Value = value;
        }
    }
}