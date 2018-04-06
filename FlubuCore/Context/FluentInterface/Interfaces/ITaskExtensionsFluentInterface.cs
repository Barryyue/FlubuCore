﻿using System;
using FlubuCore.Tasks.Packaging;
using FlubuCore.Tasks.Process;
using FlubuCore.Tasks.Versioning;

namespace FlubuCore.Context.FluentInterface.Interfaces
{
    public interface ITaskExtensionsFluentInterface
    {
        /// <summary>
        /// Generate's common assembly info file. Information is taken from <see cref="BuildProps"/>
        /// </summary>
        /// <returns></returns>
        [Obsolete("TaskExtensions fluent interface is obsolete all extensions were migrated to Tasks  fluent interface with same name as now as extension methods or new task.", true)]
        ITaskExtensionsFluentInterface GenerateCommonAssemblyInfo(Action<GenerateCommonAssemblyInfoTask> action = null);

        /// <summary>
        /// Run's multriple programs
        /// </summary>
        /// <param name="programs"></param>
        /// <returns></returns>
        [Obsolete("TaskExtensions fluent interface is obsolete all extensions were migrated to Tasks  fluent interface with same name as now as extension methods or new task.", true)]
        ITaskExtensionsFluentInterface RunMultiProgram(params string[] programs);

        /// <summary>
        /// Run specified program.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="workingFolder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [Obsolete("TaskExtensions fluent interface is obsolete all extensions were migrated to Tasks  fluent interface with same name as now as extension methods or new task.", true)]
        ITaskExtensionsFluentInterface RunProgram(string program, string workingFolder, params string[] args);

        /// <summary>
        /// Run specified program.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="workingFolder"></param>
        /// <returns></returns>
        [Obsolete("TaskExtensions fluent interface is obsolete all extensions were migrated to Tasks  fluent interface with same name as now as extension methods or new task.", true)]
        ITaskExtensionsFluentInterface RunProgram(string program, string workingFolder, Action<IRunProgramTask> action = null);

        /// <summary>
        ///  Moves back to target fluent interface.
        /// </summary>
        /// <returns></returns>
        ITargetFluentInterface BackToTarget();
    }
}
