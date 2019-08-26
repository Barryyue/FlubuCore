
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
     public partial class DockerContextUpdateTask : ExternalProcessTaskBase<int, DockerContextUpdateTask>
     {
        private string _context;

        
        public DockerContextUpdateTask(string context)
        {
            ExecutablePath = "docker";
            WithArguments("context update");
_context = context;

        }

        protected override string Description { get; set; }
        
        /// <summary>
        /// Default orchestrator for stack operations to use with this context (swarm|kubernetes|all)

        /// </summary>
        [ArgKey("--default-stack-orchestrator")]
        public DockerContextUpdateTask DefaultStackOrchestrator(string defaultStackOrchestrator)
        {
            WithArgumentsKeyFromAttribute(defaultStackOrchestrator.ToString());
            return this;
        }
        
        /// <summary>
        /// Description of the context
        /// </summary>
        [ArgKey("--description")]
        public DockerContextUpdateTask DockerDescription(string description)
        {
            WithArgumentsKeyFromAttribute(description.ToString());
            return this;
        }

        /// <summary>
        /// set the docker endpoint
        /// </summary>
        [ArgKey("--docker")]
        public DockerContextUpdateTask Docker(string docker)
        {
            WithArgumentsKeyFromAttribute(docker.ToString());
            return this;
        }

        /// <summary>
        /// set the kubernetes endpoint
        /// </summary>
        [ArgKey("--kubernetes")]
        public DockerContextUpdateTask Kubernetes(string kubernetes)
        {
            WithArgumentsKeyFromAttribute(kubernetes.ToString());
            return this;
        }
        protected override int DoExecute(ITaskContextInternal context)
        {
             WithArguments(_context);

            return base.DoExecute(context);
        }

     }
}
