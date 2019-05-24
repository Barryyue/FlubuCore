﻿using System;
using System.Collections.Generic;
using System.Text;
using FlubuCore.Context;
using Renci.SshNet.Messages;

namespace FlubuCore.Tasks.NetCore
{
    public class DotnetTestTask : ExecuteDotnetTaskBase<DotnetTestTask>
    {
        private string _description;

        public DotnetTestTask()
            : base(StandardDotnetCommands.Test)
        {
        }

        protected override string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                {
                    return $"Executes dotnet command Test";
                }

                return _description;
            }

            set { _description = value; }
        }

        /// <summary>
        /// The MSBuild project file to publish. If a project file is not specified, MSBuild searches the current working directory for a file that has a file extension that ends in `proj` and uses that file.
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public DotnetTestTask Project(string projectName)
        {
            InsertArgument(0, projectName);
            return this;
        }

        /// <summary>
        /// Looks for test binaries for a specific framework
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        public DotnetTestTask Framework(string framework)
        {
            WithArgumentsValueRequired("-f", framework);
            return this;
        }

        /// <summary>
        /// Directory in which to find the binaries to be run
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public DotnetTestTask OutputDirectory(string directory)
        {
            WithArgumentsValueRequired("-o", directory);
            return this;
        }

        /// <summary>
        /// Settings to use when running tests.
        /// </summary>
        /// <param name="settingFilePath"></param>
        /// <returns></returns>
        public DotnetTestTask SetSettingFileToUse(string settingFilePath)
        {
            WithArgumentsValueRequired("--settings", settingFilePath);
            return this;
        }

        /// <summary>
        /// Use custom adapters from the given path in the test run.
        /// </summary>
        /// <param name="pathToAdapter"></param>
        /// <returns></returns>
        public DotnetTestTask SetTestAdapterPath(string pathToAdapter)
        {
            WithArgumentsValueRequired("--test-adapter-path", pathToAdapter);
            return this;
        }

        /// <summary>
        /// Run tests that match the given expression.
        /// Examples:
        /// Run tests with priority set to 1: --filter "Priority = 1"
        /// Run a test with the specified full name: --filter "FullyQualifiedName=Namespace.ClassName.MethodName"
        /// Run tests that contain the specified name: --filter "FullyQualifiedName~Namespace.Class"
        /// More info on filtering support: https://aka.ms/vstest-filtering
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public DotnetTestTask AddFilter(string filterExpression)
        {
            WithArgumentsValueRequired("--filter", filterExpression);
            return this;
        }

        /// <summary>
        /// Configuration to use for building the project. Default for most projects is  "Debug".
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public DotnetTestTask Configuration(string configuration)
        {
            WithArgumentsValueRequired("-c", configuration);
            return this;
        }

        /// <summary>
        /// Enable verbose logs for test platform. Logs are written to the provided file.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public DotnetTestTask VerboseLogs(string pathToFile)
        {
            WithArgumentsValueRequired("-d", pathToFile);
            return this;
        }

        /// <summary>
        /// Do not build project before testing.
        /// </summary>
        /// <returns></returns>
        public DotnetTestTask NoBuild()
        {
            WithArguments("--no-build");
            return this;
        }

        /// <summary>
        /// Set the verbosity level of the command.
        /// </summary>
        /// <param name="verbosity"></param>
        /// <returns></returns>
        public DotnetTestTask Verbosity(VerbosityOptions verbosity)
        {
            WithArguments("--verbosity", verbosity.ToString().ToLower());
            return this;
        }

        /// <summary>
        /// The directory where the test results are going to be placed. The specified directory will be created if it does not exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DotnetTestTask ResultDirectory(string path)
        {
            WithArguments("-r", path);
            return this;
        }

        protected override void BeforeExecute(ITaskContextInternal context)
        {
            var args = GetArguments();
            if (args.Count == 0 || args[0].StartsWith("-"))
            {
                var solustionFileName = context.Properties.Get<string>(BuildProps.SolutionFileName, null);
                if (solustionFileName != null)
                {
                    Project(solustionFileName);
                }
            }

            if (!args.Exists(x => x == "-c" || x == "--configuration"))
            {
                var configuration = context.Properties.Get<string>(BuildProps.BuildConfiguration, null);
                if (configuration != null)
                {
                    Configuration(configuration);
                }
            }
        }
    }
}