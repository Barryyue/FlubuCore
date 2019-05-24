﻿using System;
using System.Collections.Generic;
using System.Text;
using FlubuCore.Context;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlubuCore.Tasks.NetCore
{
    public class DotnetToolInstall : ExecuteDotnetTaskBase<DotnetToolInstall>
    {
        private readonly string _nugetPackageId;

        private string _description;

        /// <summary>
        /// Installs a tool for use on the command line.
        /// </summary>
        /// <param name="nugetPackageId">NuGet Package Id of the tool to install.</param>
        public DotnetToolInstall(string nugetPackageId)
            : base(StandardDotnetCommands.Tool)
        {
            _nugetPackageId = nugetPackageId;
            WithArguments("install");
            WithArguments(nugetPackageId);
        }

        protected override string Description
        {
            get => string.IsNullOrEmpty(_description) ? "Executes command 'dotnet tool install' with specified arguments." : _description;

            set => _description = value;
        }

        /// <summary>
        /// Version of the tool package in NuGet.
        /// </summary>
        /// <param name="version">The version</param>
        /// <returns></returns>
        public DotnetToolInstall NugetPackageVersion(string version)
        {
            WithArgumentsValueRequired("--version", version);
            return this;
        }

        /// <summary>
        ///  Install user wide as global tool.
        /// </summary>
        /// <returns></returns>
        public DotnetToolInstall Global()
        {
            WithArguments("-g");
            return this;
        }

        /// <summary>
        /// Location where the tool will be installed.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DotnetToolInstall ToolInstallationPath(string path)
        {
            WithArgumentsValueRequired("--tool-path", path);
            return this;
        }

        /// <summary>
        /// The NuGet configuration file to use.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public DotnetToolInstall NugetConfigFile(string pathToFile)
        {
            WithArgumentsValueRequired("--configfile", pathToFile);
            return this;
        }

        /// <summary>
        /// Adds an additional NuGet package source to use during installation.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DotnetToolInstall AddNugetSource(string source)
        {
            WithArgumentsValueRequired("--add-source", source);
            return this;
        }

        /// <summary>
        /// The target framework to install the tool for.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        public DotnetToolInstall Framework(string framework)
        {
            WithArgumentsValueRequired("--framework", framework);
            return this;
        }

        /// <summary>
        /// Set the verbosity level of the command.
        /// </summary>
        /// <param name="verbosity"></param>
        /// <returns></returns>
        public DotnetToolInstall Verbosity(VerbosityOptions verbosity)
        {
            WithArguments("--verbosity", verbosity.ToString().ToLower());
            return this;
        }

        protected override int DoExecute(ITaskContextInternal context)
        {
            _nugetPackageId.MustNotBeNullOrEmpty("Nuget package id of the tool to install must not be empty.");
            return base.DoExecute(context);
        }
    }
}
