# nunitappdomainexample

An example project showing NUnit appdomain options and the desired behavior exhibited by xUnit.net for comparison. A setup is shown for VS adapters and console runners (in build.cake).

## Background

This project contains a very basic library using [CefSharp](https://github.com/cefsharp/CefSharp), a managed wrapper around [CEF](https://bitbucket.org/chromiumembedded/cef), an embedded browser based on Chromium.

The library in this use case, CefSharp, has several requirements. It is required that CEF is initialized and shutdown on the same thread, and in particular that it is loaded in the primary/default appdomain of the process. This is due to some technical limitations of CefSharp because it is written in Managed C++/CLI.

## Comparison

xUnit.net can handle this situation somewhat gracefully both in Visual Studio and from the command line. All that is required is the "xunit.appDomain" setting in the app.config of the test project is set to "denied".

NUnit on the other hand doesn't seem to have a similar option that can be used by the Visual Studio adapter. In fact, something deeper in the loading process of NUnit causes the runner to crash before running any tests.

Using the console runners for both frameworks is possible, so I have shown that in the build.cake script. The setup is somewhat more complicated for NUnit because it won't automatically load assemblies outside of the tools base directory and the working directory is not modified. However with some setup it does work and tests pass.

# Running tests in Visual Studio

There should be 6 total tests (3 xunit and 3 nunit) tests in the test explorer. The xunit tests should pass, however the nunit ones will crash the runner process. The exception can be seen in the Output window under Tests.

## Running tests from command line

In Powershell,

> .\build.ps1

This should show 3 passing tests for both xUnit.net and NUnit.