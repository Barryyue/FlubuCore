﻿using System;
using FlubuCore.Context;
using FlubuCore.Scripting;
using FlubuCore.Tasks.Nuget;
using System.IO;

public class MyBuildScript : DefaultBuildScript
{
    protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
    {
        context.Properties.Set(BuildProps.CompanyName, "Flubu");
        context.Properties.Set(BuildProps.CompanyCopyright, "Copyright (C) 2010-2016 Flubu");
        context.Properties.Set(BuildProps.ProductId, "FlubuCore");
        context.Properties.Set(BuildProps.ProductName, "FlubuCore");
        context.Properties.Set(BuildProps.BuildDir, "output");
        context.Properties.Set(BuildProps.SolutionFileName, "flubu.sln");
        context.Properties.Set(BuildProps.BuildConfiguration, "Release");
    }

    protected override void ConfigureTargets(ITaskContext context)
    {
        var buildVersion = context.CreateTarget("buildVersion")
            .SetAsHidden()
            .SetDescription("Fetches flubu version from FlubuCore.ProjectVersion.txt file.")
            .AddTask(x => x.FetchBuildVersionFromFileTask());

        var compile = context
            .CreateTarget("compile")
            .SetDescription("Compiles the VS solution")
            .AddCoreTask(x => x.UpdateNetCoreVersionTask("FlubuCore/FlubuCore.csproj", "dotnet-flubu/dotnet-flubu.csproj", "Flubu.Tests/Flubu.Tests.csproj", "FlubuCore.WebApi.Model/FlubuCore.WebApi.Model.csproj", "FlubuCore.WebApi.Client/FlubuCore.WebApi.Client.csproj"))
            .AddCoreTask(x => x.Restore())
            .AddCoreTask(x => x.Build())
            .DependsOn(buildVersion);

        var pack = context.CreateTarget("pack")
            .SetDescription("Packs flubu componets for nuget publishing")
            .AddCoreTask(x => x.Pack()
                .Project("FlubuCore.WebApi.Model")
                .OutputDirectory("..\\output"))
            .AddCoreTask(x => x.Pack()
                .Project("FlubuCore.WebApi.Client")
                .OutputDirectory("..\\output"))
            .AddCoreTask(x => x.Pack()
                .Project("FlubuCore")
                .OutputDirectory("..\\output"))
            .AddCoreTask(x => x.Pack()
                .Project("dotnet-flubu")
                .OutputDirectory("..\\output"))
            .DependsOn(buildVersion);

        var publishWebApi = context.CreateTarget("Publish.WebApi")
            .SetDescription("Publishes flubu web api for deployment")
            .AddCoreTask(x => x.Publish("FlubuCore.WebApi").Framework("netcoreapp2.0"))
            .AddCoreTask(x => x.Publish("FlubuCore.WebApi").Framework("netcoreapp1.1"));

        var packageWebApi = context.CreateTarget("Package.WebApi")
            .SetDescription("Prepares flubu web api deployment package.")
            .AddTask(x => x.PackageTask("output")
                .AddDirectoryToPackage(@"FlubuCore.WebApi\bin\Release\netcoreapp1.1\publish", "FlubuCore.WebApi", true)
                .AddFileToPackage("BuildScript\\DeploymentScript.cs", "")
                .AddFileToPackage("BuildScript\\DeploymentConfig.json", "")
                .AddFileToPackage("BuildScript\\Deploy.csproj", "")
                .AddFileToPackage("BuildScript\\Deploy.bat", "")
                .AddFileToPackage(@"packages\Newtonsoft.Json.10.0.2\lib\netstandard1.3\Newtonsoft.Json.dll", "lib")
                .DisableLogging()
                .ZipPackage("FlubuCore.WebApi-NetCoreApp1.1", true))
            .AddTask(x => x.PackageTask("output")
                .AddDirectoryToPackage(@"FlubuCore.WebApi\bin\Release\netcoreapp2.0\publish", "FlubuCore.WebApi", true)
                .AddFileToPackage("BuildScript\\DeploymentScript.cs", "")
                .AddFileToPackage("BuildScript\\DeploymentConfig.json", "")
                .AddFileToPackage("BuildScript\\Deploy.csproj", "")
                .AddFileToPackage("BuildScript\\Deploy.bat", "")
                .AddFileToPackage(@"packages\Newtonsoft.Json.10.0.2\lib\netstandard1.3\Newtonsoft.Json.dll", "lib")
                .DisableLogging()
                .ZipPackage("FlubuCore.WebApi-NetCoreApp2.0", true));

        var flubuRunnerMerge = context.CreateTarget("merge")
            .SetDescription("Merge's all assemblyes into .net flubu console application")
            .Do(TargetMerge);

	    var flubuTests = context.CreateTarget("test")
		    .SetDescription("Runs all tests in solution.")
		    .AddCoreTaskAsync(x => x.Test().Project("Flubu.Tests\\Flubu.Tests.csproj"))
		    .AddCoreTaskAsync(x => x.Test().Project("FlubuCore.WebApi.Tests\\FlubuCore.WebApi.Tests.csproj"));

		var nugetPublish = context.CreateTarget("nuget.publish")
            .Do(PublishNuGetPackage).
            DependsOn(buildVersion);

        var packageFlubuRunner = context.CreateTarget("package.FlubuRunner")
            .SetDescription("Packages .net 4.62 Flubu runner into zip.")
            .Do(TargetPackageFlubuRunner);

        var packageDotnetFlubu = context.CreateTarget("package.DotnetFlubu")
            .SetDescription("Packages .net 4.62 dotnet-flubu tool into zip.")
            .Do(TargetPackageDotnetFlubu);

        context.CreateTarget("rebuild")
            .SetDescription("Rebuilds the solution")
            .SetAsDefault()
            .DependsOn(compile, flubuTests);

        context.CreateTarget("rebuild.server")
            .SetDescription("Rebuilds the solution and publishes nuget packages.")
            .DependsOn(compile, flubuTests)
            .DependsOnAsync(pack, publishWebApi)
            .DependsOn(flubuRunnerMerge)
            .DependsOn(packageFlubuRunner)
            .DependsOn(packageDotnetFlubu)
            .DependsOn(packageWebApi);
            ////.DependsOn(packageWebApiWin);

        var compileLinux = context
            .CreateTarget("compile.linux")
            .SetDescription("Compiles the VS solution")
            .AddCoreTask(x => x.UpdateNetCoreVersionTask("FlubuCore/FlubuCore.csproj", "dotnet-flubu/dotnet-flubu.csproj", "Flubu.Tests/Flubu.Tests.csproj"))
            .AddCoreTask(x => x.Restore())
            .DependsOn(buildVersion);

        context.CreateTarget("rebuild.linux")
            .SetDescription("Rebuilds the solution and publishes nuget packages.")
            .DependsOn(compileLinux, flubuTests);
    }

    private static void TargetPackageFlubuRunner(ITaskContext context)
    {
         context.Tasks().PackageTask("output")
            .AddFileToPackage(@"output\build.exe", "flubu.runner")
            .AddFileToPackage(@"output\build.exe.config", "flubu.runner")
            .AddFileToPackage(@"output\flubucore.dll", "flubu.runner")
            .ZipPackage("Flubu runner", true)
            .Execute(context);
    }

    private static void TargetPackageDotnetFlubu(ITaskContext context)
    {
        context.CoreTasks().Publish("dotnet-flubu").Framework("netcoreapp2.0").Execute(context);
        if (!Directory.Exists(@"output\dotnet-flubu"))
        {
            Directory.CreateDirectory(@"output\dotnet-flubu");
        }

        context.Tasks().PackageTask(@"output\dotnet-flubu")
            .AddDirectoryToPackage(@"dotnet-flubu\bin\release\netcoreapp2.0\publish", "")
            .ZipPackage("dotnet-flubu", true)
            .Execute(context);
    }

    private static void PublishNuGetPackage(ITaskContext context)
    {
        var version = context.Properties.GetBuildVersion();
        var nugetVersion = version.ToString(3);

        try
        {
            context.CoreTasks().ExecuteDotnetTask("nuget")
                .WithArguments("push")
                .WithArguments($"output\\FlubuCore.WebApi.Model.{nugetVersion}.nupkg")
                .WithArguments("-s", "https://www.nuget.org/api/v2/package")
                .WithArguments("-k", "8da65a4d-9409-4d1b-9759-3b604d7a34ae").Execute(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to publish FlubuCore.WebApi.Model. exception: {e.Message}");
        }

        try
        {
            context.CoreTasks().ExecuteDotnetTask("nuget")
                .WithArguments("push")
                .WithArguments($"output\\FlubuCore.WebApi.Client.{nugetVersion}.nupkg")
                .WithArguments("-s", "https://www.nuget.org/api/v2/package")
                .WithArguments("-k", "8da65a4d-9409-4d1b-9759-3b604d7a34ae").Execute(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to publish FlubuCore.WebApi.Client. exception: {e.Message}");
        }

        try
        {
            context.CoreTasks().ExecuteDotnetTask("nuget")
                .WithArguments("push")
                .WithArguments($"output\\FlubuCore.{nugetVersion}.nupkg")
                .WithArguments("-s", "https://www.nuget.org/api/v2/package")
                .WithArguments("-k", "8da65a4d-9409-4d1b-9759-3b604d7a34ae").Execute(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to publish FlubuCore. exception: {e.Message}");
        }

        try
        {
            context.CoreTasks().ExecuteDotnetTask("nuget")
                .WithArguments("push")
                .WithArguments($"output\\dotnet-flubu.{nugetVersion}.nupkg")
                .WithArguments("-s", "https://www.nuget.org/api/v2/package")
                .WithArguments("-k", "8da65a4d-9409-4d1b-9759-3b604d7a34ae").Execute(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to publish dotnet-flubu. exception: {e.Message}");
        }

        try
        {
            var task = context.Tasks().PublishNuGetPackageTask("FlubuCore.Runner", @"Nuget\FlubuCoreRunner.nuspec");
            task.NugetServerUrl("https://www.nuget.org/api/v2/package")
                .ForApiKeyUse("8da65a4d-9409-4d1b-9759-3b604d7a34ae")
                .PushOnInteractiveBuild()
                .Execute(context);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to publish flubu.ruuner. exception: {e}");
        }
    }


    private static void TargetMerge(ITaskContext context)
    {
        var progTask = context.Tasks().RunProgramTask(@"tools\LibZ.Tool\1.2.0\tools\libz.exe");

        progTask
            .WorkingFolder(@"dotnet-flubu\bin\Release\net462\win7-x64")
            .WithArguments("add")
            .WithArguments("--libz", "Assemblies.libz")
            .WithArguments("--include", "*.dll")
            .WithArguments("--exclude", "FlubuCore.dll")
            .WithArguments("--move")
            .Execute(context);

        progTask = context.Tasks().RunProgramTask(@"tools\LibZ.Tool\1.2.0\tools\libz.exe");

        progTask
            .WorkingFolder(@"dotnet-flubu\bin\Release\net462\win7-x64")
            .WithArguments("inject-libz")
            .WithArguments("--assembly", "dotnet-flubu.exe")
            .WithArguments("--libz", "Assemblies.libz")
            .WithArguments("--move")
            .Execute(context);

        progTask = context.Tasks().RunProgramTask(@"tools\LibZ.Tool\1.2.0\tools\libz.exe");

        progTask
            .WorkingFolder(@"dotnet-flubu\bin\Release\net462\win7-x64")
            .WithArguments("instrument")
            .WithArguments("--assembly", "dotnet-flubu.exe")
            .WithArguments("--libz-resources")
            .Execute(context);

        context.Tasks()
            .CopyFileTask(@"dotnet-flubu\bin\Release\net462\win7-x64\dotnet-flubu.exe", @"output\build.exe", true)
            .Execute(context);
        context.Tasks()
            .CopyFileTask(@"dotnet-flubu\bin\Release\net462\win7-x64\dotnet-flubu.exe.config", @"output\build.exe.config", true)
            .Execute(context);

        context.Tasks()
            .CopyFileTask(@"dotnet-flubu\bin\Release\net462\win7-x64\FlubuCore.dll", @"output\FlubuCore.dll", true)
            .Execute(context);
        context.Tasks()
            .CopyFileTask(@"dotnet-flubu\bin\Release\net462\win7-x64\FlubuCore.xml", @"output\FlubuCore.xml", true)
            .Execute(context);
        context.Tasks()
            .CopyFileTask(@"dotnet-flubu\bin\Release\net462\win7-x64\FlubuCore.pdb", @"output\FlubuCore.pdb", true)
            .Execute(context);
    }
}