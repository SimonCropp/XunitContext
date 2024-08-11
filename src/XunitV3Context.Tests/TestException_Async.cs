﻿// #if DEBUG
// public class TestException_Async:
//     XunitContextBase
// {
//     [Fact]
//     public async Task Root()
//     {
//         await Task.Delay(1);
//         throw new("root");
//     }
//
//     [Fact]
//     public async Task AssertThrows()
//     {
//         await Task.Delay(1);
//         Assert.Throws<Exception>(MethodThatThrows);
//     }
//
//     [Fact]
//     public async Task Caught()
//     {
//         await Task.Delay(1);
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
//     public async Task FailedAssert()
//     {
//         await Task.Delay(1);
//         Assert.True(false);
//     }
//
//     [Fact]
//     public async Task Nested()
//     {
//         await Task.Delay(1);
//         MethodThatThrows();
//     }
//
//     static void MethodThatThrows() =>
//         throw new("nested");
//
//     public TestException_Async(ITestOutputHelper output) :
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

