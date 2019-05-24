﻿using System;
using Microsoft.Extensions.Logging;

namespace FlubuCore.Tests
{
    public class FlubuTestFixture : IDisposable
    {
        public FlubuTestFixture()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole((s, l) => l >= LogLevel.Information);
        }

        public ILoggerFactory LoggerFactory { get; }

        public void Dispose()
        {
        }
    }
}
