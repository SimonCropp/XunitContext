﻿// #if DEBUG
// public class TestException_Sync :
//     XunitContextBase
// {
//     [Fact]
//     public void Root() =>
//         throw new("root");
//
//     [Fact]
//     public void AssertThrows() =>
//         Assert.Throws<Exception>(MethodThatThrows);
//
//     [Fact]
//     public void Caught()
//     {
//         try
//         {
//             MethodThatThrows();
//         }
//         catch
//         {
//         }
//     }
//
//     [Fact]
//     public void FailedAssert() =>
//         Assert.True(false);
//
//     [Fact]
//     public void Nested() =>
//         MethodThatThrows();
//
//     static void MethodThatThrows() =>
//         throw new("nested");
//
//     public TestException_Sync(ITestOutputHelper output) :
//         base(output)
//     {
//     }
//
//     public override void Dispose()
//     {
//         var theExceptionThrownByTest = Context.TestException;
//         base.Dispose();
//     }
// }
// #endif

