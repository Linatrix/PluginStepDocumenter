using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PluginStepDocumenter.Library
{
    public static class PluginStepDocumentBuilder
    {
        public static string BuildPluginDocumentation(IOrganizationService service, string pluginAssemblyName)
        {
            try
            {
                var types = service.RetrieveAllWithQuery<Entity>(StepQueries.GetPluginTypes(pluginAssemblyName));
                var steps = service.RetrieveAllWithQuery<Entity>(StepQueries.GetPluginSteps(pluginAssemblyName));
                var images = service.RetrieveAllWithQuery<Entity>(StepQueries.GetPluginImages(pluginAssemblyName));
                var filters = service.RetrieveAllWithQuery<Entity>(StepQueries.GetSdkMessageFilters(pluginAssemblyName));

                PluginModel model = new PluginModel();
                model.PluginAssemblyName = pluginAssemblyName;

                List<Plugin> plugins = new List<Plugin>();

                foreach (var type in types)
                {
                    List<PluginStep> stepsForPlugin = new List<PluginStep>();

                    Plugin plugin = new Plugin();
                    plugin.Name = type.GetAttributeValue<string>(LogicalNames.PluginTypeName);

                    var pluginSteps = steps.Where(step =>
                        step.GetAttributeValue<EntityReference>(LogicalNames.StepPluginTypeId).Id == type.Id).ToList();

                    foreach (var pluginStep in pluginSteps)
                    {
                        List<PluginImage> imagesForStep = new List<PluginImage>();
                        var filter = filters.FirstOrDefault(stepFilter =>
                            stepFilter.GetAttributeValue<Guid>(LogicalNames.StepSdkMessageFilterId) ==
                            pluginStep.GetAttributeValue<EntityReference>(LogicalNames.StepSdkMessageFilterId)?.Id);

                        PluginStep step = new PluginStep();
                        //Gets the message name from the releated SDK Step Messages name
                        step.Message = pluginStep.GetAttributeValue<EntityReference>(LogicalNames.StepSdkMessageId).Name;
                        //Gets the primary entity filtering from the steps filter or returns "none" if empty as there's no filtering
                        step.PrimaryEntity = String.IsNullOrEmpty(filter?.GetAttributeValue<string>(LogicalNames.FilterPrimaryObjectTypeCode)) ?
                            "none" :
                            filter.GetAttributeValue<string>(LogicalNames.FilterPrimaryObjectTypeCode);
                        //Gets the secondary entity filtering from the steps filter or returns "none" if empty as there's no filtering
                        step.SecondaryEntity = String.IsNullOrEmpty(filter?.GetAttributeValue<string>(LogicalNames.FilterSecondaryObjectTypeCode)) ?
                            "none" :
                            filter.GetAttributeValue<string>(LogicalNames.FilterSecondaryObjectTypeCode);
                        //Gets the steps filtering attributes as a comma separated list or returns "none" if there are no filters applied
                        step.FilteringAttributes = String.IsNullOrEmpty(pluginStep.GetAttributeValue<string>(LogicalNames.StepFilteringAttributes)) ?
                            "none" :
                            pluginStep.GetAttributeValue<string>(LogicalNames.StepFilteringAttributes);
                        //Gets the steps name
                        step.StepName = pluginStep.GetAttributeValue<string>(LogicalNames.StepName);
                        //Gets the name of the user that this plugin step is set to run as, if no user is defined then returns "Calling User"
                        step.RunInUsersContext = pluginStep.GetAttributeValue<EntityReference>(LogicalNames.StepImpersonatingUserId) != null ?
                            pluginStep.GetAttributeValue<EntityReference>(LogicalNames.StepImpersonatingUserId).Name :
                            "Calling User";
                        //Gets the numeric order which this plugin step is to be executed
                        step.ExecutionOrder = pluginStep.GetAttributeValue<int?>(LogicalNames.StepRank).HasValue ?
                            pluginStep.GetAttributeValue<int?>(LogicalNames.StepRank).Value :
                            1;
                        //Gets the steps description
                        step.Description = pluginStep.GetAttributeValue<string>(LogicalNames.StepDescription);
                        //Gets the steps executing pipeline stage, this will be either "PreValidation", "PreOperation", "PostOperation" or "Stage Undefined"
                        step.EventPipelineStage = GetStageString(pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepStage));
                        //Gets the execution mode for the step, return "Asynchronous" or "Synchronous"
                        step.ExecutionMode = pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepMode)?.Value == 0 ?
                            "Asynchronous" :
                            "Synchronous";
                        //Gets whether or not this is server deployed based on the supported deployment being either Both or Server only
                        step.DeploymentServer = pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepSupportedDeployment).Value == 2 ||
                            pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepSupportedDeployment).Value == 0 ?
                            true :
                            false;
                        //Gets whether or not this is offline deployed based on the supported deployment being either both or Offline only
                        step.DeploymentOffline = pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepSupportedDeployment).Value == 2 ||
                            pluginStep.GetAttributeValue<OptionSetValue>(LogicalNames.StepSupportedDeployment).Value == 1 ?
                            true :
                            false;
                        //Gets whether the Delete Async Operations If Failure has been ticked for this plugin step
                        step.DeleteAsyncOpsIfFail = pluginStep.GetAttributeValue<bool>(LogicalNames.StepAsyncAutoDelete) == true ? true : false;

                        var pluginImages = images.Where(image =>
                            image.GetAttributeValue<EntityReference>(LogicalNames.ImageSdkMessageProcessingStepId).Id == pluginStep.Id).ToList();

                        foreach (var pluginImage in pluginImages)
                        {
                            PluginImage image = new PluginImage();
                            //Gets whether or not this image is registered as a pre image based on the image type being either both or pre only
                            image.PreImage = pluginImage.GetAttributeValue<OptionSetValue>(LogicalNames.ImageType).Value == 2 ||
                                pluginImage.GetAttributeValue<OptionSetValue>(LogicalNames.ImageType).Value == 0 ?
                                true :
                                false;
                            //Gets whether or not this image is registered as a post image based on the image type being either both or post only
                            image.PostImage = pluginImage.GetAttributeValue<OptionSetValue>(LogicalNames.ImageType).Value == 2 ||
                                pluginImage.GetAttributeValue<OptionSetValue>(LogicalNames.ImageType).Value == 1 ?
                                true :
                                false;
                            //Gets the name of the plugin image
                            image.Name = pluginImage.GetAttributeValue<string>(LogicalNames.ImageName);
                            //Gets the Entity Alias of the plugin image
                            image.EntityAlias = pluginImage.GetAttributeValue<string>(LogicalNames.ImageEntityAlias);
                            //Gets the attributes list returned in the image
                            image.Parameters = pluginImage.GetAttributeValue<string>(LogicalNames.ImageAttributes);

                            imagesForStep.Add(image);
                        }

                        step.Images = imagesForStep.ToArray();

                        stepsForPlugin.Add(step);
                    }

                    plugin.Steps = stepsForPlugin.ToArray();

                    plugins.Add(plugin);
                }

                if (plugins?.Count > 0)
                {
                    model.Plugins = plugins.ToArray();
                }

                return JsonConvert.SerializeObject(model, Formatting.Indented);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occured trying to generate JSON for plugin assembly with name: {pluginAssemblyName}, error: {ex.Message}");
            }
        }

        private static string GetStageString(OptionSetValue stage)
        {
            switch (stage?.Value)
            {
                case 10: return "PreValidation";
                case 20: return "PreOperation";
                case 40: return "PostOperation";
                default: return "Stage Undefined";
            }
        }
    }
}
