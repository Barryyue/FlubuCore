
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

namespace FlubuCore.Tasks.Docker
{
     public partial class DockerImagesTask : ExternalProcessTaskBase<int, DockerImagesTask>
     {
        private string _repository;

        
        public DockerImagesTask(string repository)
        {
            ExecutablePath = "docker";
            WithArguments("images");
_repository = repository;

        }

        protected override string Description { get; set; }
        
        /// <summary>
        /// Show all images (default hides intermediate images)
        /// </summary>
        [ArgKey("--all")]
        public DockerImagesTask All()
        {
            WithArgumentsKeyFromAttribute();
            return this;
        }

        /// <summary>
        /// Show digests
        /// </summary>
        [ArgKey("--digests")]
        public DockerImagesTask Digests()
        {
            WithArgumentsKeyFromAttribute();
            return this;
        }

        /// <summary>
        /// Filter output based on conditions provided
        /// </summary>
        [ArgKey("--filter")]
        public DockerImagesTask Filter(string filter)
        {
            WithArgumentsKeyFromAttribute(filter.ToString());
            return this;
        }

        /// <summary>
        /// Pretty-print images using a Go template
        /// </summary>
        [ArgKey("--format")]
        public DockerImagesTask Format(string format)
        {
            WithArgumentsKeyFromAttribute(format.ToString());
            return this;
        }

        /// <summary>
        /// Don't truncate output
        /// </summary>
        [ArgKey("--no-trunc")]
        public DockerImagesTask NoTrunc()
        {
            WithArgumentsKeyFromAttribute();
            return this;
        }

        /// <summary>
        /// Only show numeric IDs
        /// </summary>
        [ArgKey("--quiet")]
        public DockerImagesTask Quiet()
        {
            WithArgumentsKeyFromAttribute();
            return this;
        }
        protected override int DoExecute(ITaskContextInternal context)
        {
             WithArguments(_repository);

            return base.DoExecute(context);
        }

     }
}
