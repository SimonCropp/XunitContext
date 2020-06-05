using System;
using Xunit;
using Xunit.Abstractions;

namespace TheNamespace
{
    public class CountersTest :
        XunitContextBase
    {
        [Fact]
        public void Current()
        {
            var counter = new GuidCounter();
            Assert.Equal(counter.Current, counter.Current);
        }

        [Fact]
        public void Next()
        {
            var counter = new GuidCounter();
            var first = counter.Current;
            var next = counter.Next();
            Assert.NotEqual(first, next);
            Assert.NotEqual(first, counter.Current);
            Assert.Equal(next, counter.Current);
        }

        [Fact]
        public void IntOrNext()
        {
            var counter = new GuidCounter();
            var newGuid = Guid.NewGuid();
            var one = counter.IntOrNext(newGuid);
            var two = counter.IntOrNext(newGuid);
            Assert.Equal(one, two);
        }

        public CountersTest(ITestOutputHelper testOutput) :
            base(testOutput)
        {
        }
    }
}