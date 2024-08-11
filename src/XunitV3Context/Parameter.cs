namespace Xunit;

public class Parameter
{
    public ParameterInfo Info { get; }
    public object? Value { get; }

    public Parameter(ParameterInfo info, object? value)
    {
        Guard.AgainstNull(info, nameof(info));
        Info = info;
        Value = value;
    }
}