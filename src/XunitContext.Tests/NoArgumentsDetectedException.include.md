
> No arguments detected for method with parameters.
> This is most likely caused by using a parameter that Xunit cannot serialize.
> Instead pass in a simple type as a parameter and construct the complex object inside the test.
> Alternatively; override the current parameters using `UseParameters()` via the current test base class, or via `XunitContext.Current.UseParameters()`.

