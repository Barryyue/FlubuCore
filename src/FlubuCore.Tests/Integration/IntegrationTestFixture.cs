﻿using System;
using DotNet.Cli.Flubu.Infrastructure;
using FlubuCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlubuCore.Tests.Integration
{
    public class IntegrationTestFixture : IDisposable
    {
        public IntegrationTestFixture()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole((s, l) => l >= LogLevel.Information);
            var services = new ServiceCollection()
                .AddLogging()
                .AddCoreComponents()
                .AddScriptAnalyser()
                .AddArguments(new[] { "flubu" })
                .AddTasks();

            ServiceProvider = services.BuildServiceProvider();
        }

        public ILoggerFactory LoggerFactory { get; }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
        }
    }
}
