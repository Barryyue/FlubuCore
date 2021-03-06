
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

namespace FlubuCore.Tasks.Docker.Container
{
     public partial class DockerContainerPruneTask : ExternalProcessTaskBase<int, DockerContainerPruneTask>
     {
        
        
        public DockerContainerPruneTask()
        {
            ExecutablePath = "docker";
            WithArguments("container prune");

        }

        protected override string Description { get; set; }
        
        /// <summary>
        /// Provide filter values (e.g. 'until=<timestamp>')
        /// </summary>
        [ArgKey("--filter")]
        public DockerContainerPruneTask Filter(string filter)
        {
            WithArgumentsKeyFromAttribute(filter.ToString());
            return this;
        }

        /// <summary>
        /// Do not prompt for confirmation
        /// </summary>
        [ArgKey("--force")]
        public DockerContainerPruneTask Force()
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
