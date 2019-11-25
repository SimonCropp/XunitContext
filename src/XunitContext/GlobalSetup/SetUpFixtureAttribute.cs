using System;

namespace Xunit
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SetUpFixtureAttribute :
        Attribute
    {
    }
}