# FlubuCore

## **Introduction**
"FlubuCore - Fluent Builder Core" is a cross platform build and deployment automation system. You can define your build and deployment scripts in C# using an intuitive fluent interface. This gives you code completion, IntelliSense, debugging, FlubuCore custom analyzers, and native access to the whole .NET ecosystem inside of your scripts.

![FlubuCore in action](https://raw.githubusercontent.com/flubu-core/flubu.core/master/assets/demo.gif)

FlubuCore offers a .net (core) console application that uses power of roslyn to compile and execute scripts. Above example can be run from console with:

* FlubuCore runner  ``` flubu.exe Default ```
* FlubuCore dotnet cli tool ``` dotnet flubu Default ```
* FlubuCore global tool ``` flubu Default ```

## **Features and Advantages**

* Intuitive an easy to learn. C#, fluent interface, and IntelliSense make even most complex script creation a breeze.

```cs
context.CreateTarget("Example")
  .DependsOn(fetchBuildVersionTarget)
  .AddTask(x => x.CompileSolutionTask())
  .AddTask(x => x.PublishNuGetPackageTask("packageId", "pathToNuspec"))
      .When(c => c.BuildSystems().Jenkins().IsRunningOnJenkins);
```
          
* [Large number of often used built-in tasks](https://github.com/flubu-core/flubu.core/wiki/4-Tasks) like e.g. running tests, managing IIS, creating deployment packages, publishing NuGet packages, docker tasks, executing PowerShell scripts and many more.

```cs
target
    .AddTask(x => x.CompileSolutionTask())
    .AddTask(x => x.CopyFileTask(source, destination, true))
    .AddTask(x => x.IisTasks()
                    .CreateAppPoolTask("Example app pool")
                    .Mode(CreateApplicationPoolMode.DoNothingIfExists));
```

* [Execute your own custom C# code.](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals#Custom-code)

```cs
context.CreateTarget("MyCustomBuildTarget")
     .AddTask(x => x.CompileSolutionTask())
     .Do(MyCustomMethod)
     .Do(NuGetPackageReferencingExample);
```

* [assembly references and nuget packages are loaded automatically](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals#Referencing-other-assemblies-in-build-script) when script is used together with project file. When script is executed alone (for example when deploying with FlubuCore script on production environment) references can be added with attributes.

```cs
[NugetPackage("Newtonsoft.json", "11.0.2")]
[Assembly(".\Lib\EntityFramework.dll")]
public class BuildScript : DefaultBuildScript
{
   public void NuGetPackageReferencingExample(ITaskContext context)
    {
        JsonConvert.SerializeObject("Example");
    }
}
```

* [Easily run any external program or console command in your script.](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals#Run-any-program)

```cs
context.CreateTarget("Run.Libz")
    .AddTask(x => x.RunProgramTask(@"packages\LibZ.Tool\1.2.0\tools\libz.exe")
        .WorkingFolder(@".\src")
        .WithArguments("add")
        .WithArguments("--libz", "Assemblies.libz"));
```

* [Pass command line arguments, settings from json configuration file or environment variables to your script.](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals#Script-arguments)

```cs
 public class SimpleScript : DefaultBuildScript
 {
    [FromArg("sn", "If true app is deployed on second node. Otherwise not.")]
    public bool deployOnSecondNode { get; set; }

 
     protected override void ConfigureTargets(ITaskContext context)
     {
         context.CreateTarget("compile")
            .AddTask(x => x.CompileSolutionTask()
                .ForMember(y => y.SolutionFileName("someSolution.sln"), "solution", "The solution to build.")); 
     }
  }
 ```
 
 ```
  flubu.exe compile -solution=someOtherSolution.sln -sn=true
 ```
 
* [Extending FlubuCore fluent interface by writing your own tasks within FlubuCore plugins.](https://github.com/flubu-core/flubu.core/wiki/5-How-to-write-and-use-FlubuCore-task-plugins)

```cs
    public class ExampleFlubuPluginTask : TaskBase<int, ExampleFlubuPluginTask>
    {
        protected override int DoExecute(ITaskContextInternal context)
        {
            // Write your task logic here.
            return 0;
        }
    }
```
	
* [Growing list of FlubuCore plugins complements built in tasks.](https://github.com/flubu-core/flubu.core/wiki/90-Awesome-FlubuCore-plugins)

* [Asynchronous execution of tasks, target dependencies and custom code.](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals#Async-execution)

```cs
    context.CreateTarget("Run.Tests")
        .AddTaskAsync(x => x.NUnitTaskForNunitV3("TestProjectName1"))
        .AddTaskAsync(x => x.NUnitTaskForNunitV3("TestProjectName1"))
        .AddTaskAsync(x => x.NUnitTaskForNunitV3("TestProjectName3"));
```

* [Full .NET Core support including the global CLI tool](https://github.com/flubu-core/flubu.core/wiki/1-Getting-started#getting-started-net-core)

```
    dotnet tool install --global FlubuCore.GlobalTool
    flubu compile
```

* [Possibility to test and debug your build scripts.](https://github.com/flubu-core/flubu.core/wiki/6-Writing-build-script-tests,-debuging-and-running-flubu-tasks-in-other--.net-applications)

```cs
    context.WaitForDebugger();
```

* [Easily automate deployments remotely via the FlubuCore Web API.](https://github.com/flubu-core/flubu.core/wiki/7-Web-Api:-Getting-started)

* [Possibility to use FlubuCore tasks in any other .NET application.](https://github.com/flubu-core/examples/blob/master/NetCore_csproj/BuildScript/BuildScriptTests.cs)

* Improved developer experience with FlubuCore custom analyzers.

![FlubuCore analyzers in action](https://raw.githubusercontent.com/flubu-core/flubu.core/master/assets/FlubuCoreCustomAnalyzerDemo.png)

## **Getting Started**
Using FlubuCore is straightforward and very simple :-) It is also fully and throughly documented.

The [Getting Started](https://github.com/flubu-core/flubu.core/wiki/1-Getting-started) chapter on [FlubuCore Wiki](https://github.com/flubu-core/flubu.core/wiki/) will help you set up your first FlubuCore build in no time.

A comprehensive list of features that FlubuCore has to offer with descriptions can be found in the [Build Script Fundamentals](https://github.com/flubu-core/flubu.core/wiki/2-Build-script-fundamentals) chapter.

Once you have your build and deployment scripts defined, the following Wiki chapters will explain how to run them:
* For .NET Framework projects use [FlubuCore.Runner](https://github.com/flubu-core/flubu.core/wiki/1-Getting-started#Installation.net)
* For .NET Core projects use [FlubuCore CLI global tool](https://github.com/flubu-core/flubu.core/wiki/1-Getting-started#Installation-.net-core)

## **Examples**
Aside from the detailed Wiki FlubuCore comes with example projects that reflect real-life situations. The examples can be found in the separate [Examples repository](https://github.com/flubu-core/examples/).

These examples will help you to get quickly start with FlubuCore:
* [.NET Framework build example](https://github.com/flubu-core/examples/blob/master/MVC_NET4.61/BuildScripts/BuildScript.cs
)

* [.NET Core build example](https://github.com/flubu-core/examples/blob/master/NetCore_csproj/BuildScript/BuildScript.cs
)

* [Deployment script example](https://github.com/flubu-core/examples/blob/master/DeployScriptExample/BuildScript/DeployScript.cs
)

## **Have a question?**

 [![Join the chat at https://gitter.im/FlubuCore/Lobby](https://badges.gitter.im/mbdavid/LiteDB.svg)](https://gitter.im/FlubuCore/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## **Contributing**

Please see [CONTRIBUTING.md](https://github.com/dotnetcore/FlubuCore/blob/master/CONTRIBUTING.md).

## **Ways to Contribute**

* Spread the word about the project.
* If you like the project don't forget to give it a star so that the community get's bigger.
* Improve documentation.
* Report, fix a bug.
* Implement a new feature.
* Discuss potential ways to improve project.
* Improve existing implementation, performance, etc.

## **Changelog and Roadmap**

Changes with description and examples can be found in [Changelog](https://github.com/flubu-core/flubu.core/blob/master/CHANGELOG.md) 
 
You can see FlubuCore roadmap by exploring opened [milestones.](https://github.com/flubu-core/flubu.core/milestones)
