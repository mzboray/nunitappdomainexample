#tool nuget:?package=NUnit.ConsoleRunner&version=3.11.1
#tool nuget:?package=xunit.runner.console&version=2.4.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "./TestAppDomainExample.sln";
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/bin/**" + configuration);
});

Task("Restore-NuGetPackages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGetPackages")
    .Does(() =>
{
      // Use MSBuild
      MSBuild(
          solution,
          settings => settings.SetConfiguration(configuration));
});

Task("Run-UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2(
        "./XUnitTest/bin/x64/" + configuration + "/XUnitTest.dll");

    // nunit can't load dlls from the test directory with
    // the domain usage option set to none, so we copy the tools to
    // the proper location and set the tool path
    var nunitConsoleDirectory = GetFiles("./tools/**/nunit3-console.exe")
        .First().GetDirectory();       
    string targetDirectory = "./NUnitTest/bin/x64/" + configuration;
    CopyDirectory(nunitConsoleDirectory, targetDirectory);

    NUnit3(
        targetDirectory + "/NUnitTest.dll",
        new NUnit3Settings
        {
            NoResults = true,
            AppDomainUsage = NUnit3AppDomainUsage.None,
            ToolPath = targetDirectory + "/nunit3-console.exe",
            // Setting the working directory for either tool to work 
            // is not required, but it prevents our root folder from 
            // being cluttered by Cef browser cache folders
            WorkingDirectory = targetDirectory
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-UnitTests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target)