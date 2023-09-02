# Pgnoli
Pgnoli is a .NET implementation of the PostgreSQL wire protocol v3. 
It gives you some traits to implement your own PostgreSQL compatible servers (and client if there is case [Npgsql](https://github.com/Npgsql/Npgsql) does not fit).

Postgresql wire protocol is actually a general purpose Layer-7 application protocol. It is not bound with SQL, and can be used for any query language, data format or even natural language. It is possible to build a chat bot that use psql as client.

- [X] (De)serialization of messages for Startup flow
- [X] (De)serialization of messages for Simple Query flow
- [X] (De)serialization of messages for Extended Query flow
- [X] (De)serialization of types for binary encoding
- [X] (De)serialization of types for text encoding
- [X] Support of DateStyle for encoding Date/Time related types

![Logo](https://raw.githubusercontent.com/Seddryck/Pgnoli/main/assets/logo/Pgnoli-logo.png)

[About][] | [Quickstart][]

[About]: #about (About)
[Quickstart]: #quickstart (Quickstart)

## About

**Social media:** [![website](https://img.shields.io/badge/website-seddryck.github.io/Pgnoli-fe762d.svg)](https://seddryck.github.io/Pgnoli)
[![twitter badge](https://img.shields.io/badge/twitter%20Pgnoli-@Seddryck-blue.svg?style=flat&logo=twitter)](https://twitter.com/Seddryck)

**Releases:** [![nuget](https://img.shields.io/nuget/v/Pgnoli.svg)](https://www.nuget.org/packages/Pgnoli/)
[![licence badge](https://img.shields.io/badge/License-Apache%202.0-yellow.svg)](https://github.com/Seddryck/Pgnoli/blob/master/LICENSE)

**Dev. activity:** [![GitHub last commit](https://img.shields.io/github/last-commit/Seddryck/Pgnoli.svg)](https://github.com/Seddryck/Pgnoli/commits)
![Still maintained](https://img.shields.io/maintenance/yes/2023.svg)
![GitHub commit activity](https://img.shields.io/github/commit-activity/y/Seddryck/Pgnoli)

**Continuous integration builds:** [![Build status](https://ci.appveyor.com/api/projects/status/80321ajt5beih60f?svg=true)](https://ci.appveyor.com/project/Seddryck/Pgnoli/)
[![Tests](https://img.shields.io/appveyor/tests/seddryck/Pgnoli.svg)](https://ci.appveyor.com/project/Seddryck/Pgnoli/build/tests)
[![CodeFactor](https://www.codefactor.io/repository/github/seddryck/Pgnoli/badge)](https://www.codefactor.io/repository/github/seddryck/Pgnoli)
[![codecov](https://codecov.io/github/Seddryck/Pgnoli/branch/main/graph/badge.svg?token=9ZSJ6N0X9E)](https://codecov.io/github/Seddryck/Pgnoli)
<!--[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FSeddryck%2FPgnoli.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FSeddryck%2FPgnoli?ref=badge_shield)-->

**Status:** [![stars badge](https://img.shields.io/github/stars/Seddryck/Pgnoli.svg)](https://github.com/Seddryck/Pgnoli/stargazers)
[![Bugs badge](https://img.shields.io/github/issues/Seddryck/Pgnoli/bug.svg?color=red&label=Bugs)](https://github.com/Seddryck/Pgnoli/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:bug+)
[![Features badge](https://img.shields.io/github/issues/seddryck/Pgnoli/new-feature.svg?color=purple&label=Feature%20requests)](https://github.com/Seddryck/Pgnoli/issues?utf8=%E2%9C%93&q=is:issue+is:open+label:new-feature+)
[![Top language](https://img.shields.io/github/languages/top/seddryck/Pgnoli.svg)](https://github.com/Seddryck/Pgnoli/search?l=C%23)

## Quickstart

Pgnoli is a .NET assembly and can be installed as a package next to your project with the help of `Nuget CLI` or `dotnet.exe` 

```bash
dotnet add package Pgnoli
```

When receiving a message from the client your solution can use the `FrontendParser` class to deserialize the message contained in an array of bytes.

```csharp
var parser = new FrontendParser();
var msg = parser.Parse(bytes);
```

The parser returns a strongly typed message implementing the interface `IMessage`. Most of the messages inherits from the class `CodeMessage` exposing a property Code of one char.

Once the message is parsed, you can access its payload, using the property `Payload`. The content of the payload depends of the message class, i.e. for a [Query message](https://www.postgresql.org/docs/current/protocol-message-formats.html), the payload contains the SQL query.

```csharp
var queryMsg = msg as Query;
Console.WriteLine(queryMsh.Sql);
```

To create a message, you can use the static methods offered by the class implementing these messages. These methods depends of the messages, i.e. to create a message `ReadyForQuery`, you can use the static method `Idle`.
To effectively get your message you should call the `Build()` method.

```csharp
var msg = ReadyForQuery.Idle.Build();
```

Some messages accept optional informations. You can communicate them with the help of `With(...)` functions. i.e. the message `Bind` can create messages for named and unnamed portals but also accept the different parameters and how the result should be serialized. This last element can be defined with the methods `WithAllResultsAsText()` or `WithAllResultsAsBinary()` or for each column of the result, we can define the encoding by chaining the calls to `WithResultFormat(...)`.

```csharp
var msg = Bind.Portal("destination", "source").WithAllResultsAsBinary().Build();
```

The handling of the encoding in both text and binary formats is done with the classes from the `TypeHandlers` namespace.
However in most cases, you won't need them as the handling is directly managed within the class implementing the messages.