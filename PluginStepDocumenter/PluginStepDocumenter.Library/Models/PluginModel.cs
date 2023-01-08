using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PluginStepDocumenter.Library
{
    [DataContract]
    internal class PluginModel
    {
        [DataMember]
        public string PluginAssemblyName { get; set; }
        [DataMember]
        public Plugin[] Plugins { get; set; }
    }

    [DataContract]
    public class Plugin
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public PluginStep[] Steps { get; set; }
    }

    [DataContract]
    public class PluginStep
    {
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string PrimaryEntity { get; set; }
        [DataMember]
        public string SecondaryEntity { get; set; }
        [DataMember]
        public string FilteringAttributes { get; set; }
        [DataMember]
        public string StepName { get; set; }
        [DataMember]
        public string RunInUsersContext { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string EventPipelineStage { get; set; }
        [DataMember]
        public string ExecutionMode { get; set; }
        [DataMember]
        public bool DeploymentServer { get; set; }
        [DataMember]
        public bool DeploymentOffline { get; set; }
        [DataMember]
        public bool DeleteAsyncOpsIfFail { get; set; }
        [DataMember]
        public PluginImage[] Images { get; set; }
    }

    [DataContract]
    public class PluginImage
    {
        [DataMember]
        public bool PreImage { get; set; }
        [DataMember]
        public bool PostImage { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string EntityAlias { get; set; }
        [DataMember]
        public string Parameters { get; set; }
    }
}
