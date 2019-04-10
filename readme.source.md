# <img src="https://raw.github.com/SimonCropp/ObjectApproval/master/icon.png" height="40px"> ObjectApproval

Extends [ApprovalTests](https://github.com/approvals/ApprovalTests.Net) to allow simple approval of complex models using [Json.net](https://www.newtonsoft.com/json).


## The NuGet package [![NuGet Status](http://img.shields.io/nuget/v/ObjectApproval.svg?style=flat)](https://www.nuget.org/packages/ObjectApproval/)

https://nuget.org/packages/ObjectApproval/

    PM> Install-Package ObjectApproval


## Usage

Assuming you have previously verified and approved using this. 

snippet: before

Then you attempt to verify this 

snippet: after

The serialized json version of these will then be compared and you will be displayed the differences in the diff tool you have asked ApprovalTests to use. For example:

![SampleDiff](https://raw.github.com/SimonCropp/ObjectApproval/master/src/SampleDiff.png)

Note that the output is technically not valid json. [Single quotes are used](#single-quotes-used) and [names are not quoted](#quotename-is-false). The reason for this is to make the resulting output easier to read and understand.


### Validating multiple instances

When validating multiple instances, an [anonymous type](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/anonymous-types) can be used for verification

snippet: anon

Results in the following:

```graphql
{
  person1: {
    GivenNames: 'John',
    FamilyName: 'Smith'
  },
  person2: {
    GivenNames: 'Marianne',
    FamilyName: 'Aguirre'
  }
}
```


## Serializer settings

`SerializerBuilder` is used to build the Json.net `JsonSerializerSettings`. This is done for every verification run by calling `SerializerBuilder.BuildSettings()`.

All modifications of `SerializerBuilder` behavior is global for all verifications and should be done once at assembly load time.


### Default settings

The default serialization settings are:

snippet: defaultSerialization


### Single quotes used

[JsonTextWriter.QuoteChar](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonTextWriter_QuoteChar.htm) is set to single quotes `'`. The reason for this is that it makes approval files cleaner and easier to read and visualize/understand differences

To change this behavior use:

```cs
SerializerBuilder.UseDoubleQuotes = true;
```


### QuoteName is false

[JsonTextWriter.QuoteName](https://www.newtonsoft.com/json/help/html/P_Newtonsoft_Json_JsonTextWriter_QuoteName.htm) is set to false. The reason for this is that it makes approval files cleaner and easier to read and visualize/understand differences

To change this behavior use:

```cs
SerializerBuilder.QuoteNames = true;
```


### Empty collections are ignored

By default empty collections are ignored during verification.

To disable this behavior use:

```cs
SerializerBuilder.IgnoreEmptyCollections = false;
```


### Guids are scrubbed

By default guids are sanitized during verification. This is done by finding each guid and taking a counter based that that specific guid. That counter is then used replace the guid values. This allows for repeatable tests when guid values are changing.

snippet: guid

Results in the following:

```graphql
{
  Guid: 'Guid 1',
  GuidNullable: 'Guid 1',
  GuidString: 'Guid 1',
  OtherGuid: 'Guid 2'
}
```

To disable this behavior use:

```cs
SerializerBuilder.ScrubGuids = false;
```


### Dates are scrubbed

By default dates (`DateTime` and `DateTimeOffset`) are sanitized during verification. This is done by finding each date and taking a counter based that that specific date. That counter is then used replace the date values. This allows for repeatable tests when date values are changing.

snippet: Date

Results in the following:

```graphql
{
  DateTime: 'DateTime 1',
  DateTimeNullable: 'DateTime 1',
  DateTimeOffset: 'DateTimeOffset 1',
  DateTimeOffsetNullable: 'DateTimeOffset 1',
  DateTimeString: 'DateTimeOffset 2',
  DateTimeOffsetString: 'DateTimeOffset 2'
}
```

To disable this behavior use:

```cs
SerializerBuilder.ScrubDateTimes = false;
```


### Defalt Booleans are ignored

By default values of `bool` and `bool?` are ignored during verification. So properties that equate to 'false' will not be written,

To disable this behavior use:

```cs
SerializerBuilder.IgnoreFalse = false;
```


### Change defaults at the verification level

`DateTime`, `DateTimeOffset`, `Guid`, `bool`, and empty collection behavior can also be controlled at the verification level: 

snippet: ChangeDefaultsPerVerification


### Changing settings globally

To change the serialization settings for all verifications use `SerializerBuilder.ExtraSettings`:

snippet: ExtraSettings


### Ignoring a type

To ignore all members that match a certain type:

snippet: AddIgnore


### Ignore member by expressions

To ignore members of a certain type using an expression:

snippet: IgnoreMemberByExpression


### Ignore member by name

To ignore members of a certain type using type and name:

snippet: IgnoreMemberByName


### Members that throw

Members that throw exceptions can be excluded from serialization based on the exception type or properties.

By default members that throw `NotImplementedException` or `NotSupportedException` are ignored.

Note that this is global for all members on all types.

Ignore by exception type:

snippet: IgnoreMembersThatThrow


Ignore by exception type and expression:

snippet: IgnoreMembersThatThrowExpression


### Scrubber

A scrubber can be used to cleanup or sanitize the resultant serialized string prior to verification.

snippet: Scrubber

Results in the following:

```graphql
{
  RowVersion: 'ThRowVersion'
}
```


## Icon

<a href="http://thenounproject.com/term/helmet/9554/" target="_blank">Helmet</a> designed by <a href="http://thenounproject.com/alterego" target="_blank">Leonidas Ikonomou</a> from The Noun Project
