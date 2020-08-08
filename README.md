# nunitappdomainexample
An example project showing test framework appdomain options. The tests just check if the current domain is the default app domain.

NUnit can use the runsettings DisableAppDomain option but this does not use the default appdomain.
xUnit.net has the same behavior with this option.

xUnit.net has a separate configuration option appDomain=denied set in the app config (or xunit.runner.json). That causes tests to be run
in the default app domain. For this option to function, don't set the DisableAppDomain to true in the runsettings.
