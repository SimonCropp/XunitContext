using System;

namespace Xunit
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GlobalSetUpAttribute :
        Attribute
    {
    }
}