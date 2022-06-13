# Foobar Factory

This console application is built as a technical demonstration of the Foobar Factory game, with automated robots on a production line that decide to create, assemble, and sell items based on a weighted automaton.

## How to Run

The easiest way to run this app and its tests is directly from within Visual Studio.

Alternatively, you can publish it yourself for all platforms by [installing .NET](https://docs.microsoft.com/en-us/dotnet/core/install/) and using the `dotnet publish` command.

For example:

- Publish to run on OSX:
```
dotnet publish -r osx-x64
```

- Publish as self-contained to run on Linux (i.e. the running computer doesn't need .NET installed).
```
dotnet publish -r linux-x64 --self-contained
```

For more information on this topic, see the [Application Publishing article on MSDN](https://docs.microsoft.com/en-us/dotnet/core/deploying/).
