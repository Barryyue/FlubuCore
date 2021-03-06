
//-----------------------------------------------------------------------
// <auto-generated />
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using FlubuCore.Context;
using FlubuCore.Tasks;
using FlubuCore.Tasks.Attributes;
using FlubuCore.Tasks.Process;

namespace FlubuCore.Tasks.Docker.Context
{
     public partial class DockerContextLsTask : ExternalProcessTaskBase<int, DockerContextLsTask>
     {
        
        
        public DockerContextLsTask()
        {
            ExecutablePath = "docker";
            WithArguments("context ls");

        }

        protected override string Description { get; set; }
        
        /// <summary>
        /// Pretty-print contexts using a Go template
        /// </summary>
        [ArgKey("--format")]
        public DockerContextLsTask Format(string format)
        {
            WithArgumentsKeyFromAttribute(format.ToString());
            return this;
        }

        /// <summary>
        /// Only show context names
        /// </summary>
        [ArgKey("--quiet")]
        public DockerContextLsTask Quiet()
        {
            WithArgumentsKeyFromAttribute();
            return this;
        }
        protected override int DoExecute(ITaskContextInternal context)
        {
            
            return base.DoExecute(context);
        }

     }
}
