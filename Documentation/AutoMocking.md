# Auto-Mocking

## Table of contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<details>
<summary>Details</summary>

- [Mocking](#mocking)
- [Using Moq](#using-moq)
- [Using NSubstitute](#using-nsubstitute)
  - [Auto values](#auto-values)
  - [Setting return values](#setting-return-values)

</details>
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Mocking

> Test double, mock, stub, fake or spy? 
> For simplicity this documentation is using the word "mock" for all.

One of the really cool features of DI is the fact that it makes testing code much, much easier.  This is because you can easily substitute one dependency for another by using a different Composition Root. For example, if you only want to test a particular class (let's call it Foo) and don't care about testing its dependencies, you might write *mocks* for them so that you can isolate Foo specifically.

```csharp
public class Foo
{
    IWebServer _webServer;

    public Foo(IWebServer webServer)
    {
        _webServer = webServer;
    }

    public void Initialize()
    {
        //...
        var x = _webServer.GetSomething();
        //...
    }
}
```

In this example, we have a class Foo that interacts with a web server to retrieve content.  This would normally be very difficult to test for the following reasons:

* You would have to set up an environment where it can properly connect to a web server (configuring ports, urls, etc.)
* Running the test could be slower and limit how much testing you can do
* The web server itself could contain bugs so you couldn't with certainty isolate Foo as the problematic part of the test
* You can't easily configure the values returned from the web server to test sending various inputs to the Foo class

However, if we create a mock class for IWebServer then we can address all these problems:

```csharp
public class MockWebServer : IWebServer
{
    //...
}
```

Then hook it up in our installer:

```csharp
Container.Bind<IWebServer>().To<MockWebServer>().AsSingle();
```

Then you can implement the fields of the IWebServer interface and configure them based on what you want to test on Foo. Hopefully You can see how this can make life when writing tests much easier.

To avoid writing all the mocking classes, like the above MockWebServer class example. Zenject allows you to automate this process by using a mocking library which does all the work for you. Zenject supports *Moq* and *NSubstitute*. Both are most used in the field but have some different approaches to mocking. And will be handled differently in this document.

Note that by default, Auto-mocking is not enabled in Zenject.  If you wish to use the auto-mocking feature then you need to extract AutoMoq.zip or AutoSubstitute.zip as described below.

## Using Moq

If you wish to use Moq then you need to go to your Zenject install directory and extract the contents of ``Zenject\OptionalExtras\AutoMoq.zip`` into that same directory.
The extracted folder should then be moved to ``Zenject\OptionalExtras\TestFrameWork\Editor``.

Note that there are multiple versions of Moq.dll included in the zip and that you should use the one that targets the Scripting Runtime Version that you have configured in your player settings. Also note that if you're using Scripting Runtime Version 3.5, that you might also need to change your "Api Compatibility Level" from ".NET 2.0 Subset" to ".NET 2.0"

After extracting the auto mocking package it is just a matter of using the following syntax to mock out various parts of your project:

```csharp
Container.Bind<IFoo>().FromMock();
```

Or, alternatively, if we want to configure values for our mock class (rather than just have it generate defaults):

```csharp
var foo = new Mock<IFoo>();
foo.Setup(x => x.Bar).Returns("a");
Container.BindInstance(foo.Object);
```

For more details, see the documentation for [Moq](https://github.com/moq/moq4)

## Using NSubstitute

If you wish to use NSubstitute then you need to go to your Zenject install directory and extract the contents of ``Zenject\OptionalExtras\AutoSubstitute.zip``.
The extracted folder should then be moved to ``Zenject\OptionalExtras\TestFrameWork\Editor``.

> Mock, stub, fake, spy, test double? Strict or loose? Nah, just substitute for the type you need!

After extracting the auto substitute package you are ready to create a substitute with one single line of code:

```csharp
Container.Bind<ICalculator>().FromSubstitute();
```

Rather than writing:

```csharp
var calculator = Substitute.For<ICalculator>();
Container.BindInstance(calculator);
```

### Auto values

Once a substitute has been created some properties and methods will automatically return non-null values. For example, any properties or methods that return an interface, delegate, or purely virtual class* will automatically return substitutes themselves. This is commonly referred to as recursive mocking, and can be useful because you can avoid explicitly creating each substitute, which means less code. Other types, like String and Array, will default to returning empty values rather than nulls.  

### Setting return values

After the creation of a substitute, it's easy as 1, 2, 3; to set the return values of methods and properties:

```csharp  
calculator.Add(1, 2).Returns(3);
```

For more details, see the documentation for [NSUbstitute](https://nsubstitute.github.io)
