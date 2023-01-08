using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginStepDocumenter.Library
{
    internal class StepQueries
    {
        public static QueryExpression GetPluginAssemblies()
        {
            QueryExpression assemblyQuery = new QueryExpression(LogicalNames.PluginAssemblyEntityName);
            assemblyQuery.ColumnSet = new ColumnSet("name");
            assemblyQuery.AddOrder("name", OrderType.Ascending);

            return assemblyQuery;
        }

        internal static QueryExpression GetPluginTypes(string pluginAssemblyName)
        {
            var typeQuery = new QueryExpression(LogicalNames.PluginTypeEntityName);
            typeQuery.ColumnSet = new ColumnSet(true);
            typeQuery.AddOrder("friendlyname", OrderType.Ascending);

            var assemblyLink = typeQuery.AddLink("pluginassembly", "pluginassemblyid", "pluginassemblyid");
            assemblyLink.LinkCriteria.AddCondition("name", ConditionOperator.Equal, pluginAssemblyName);

            return typeQuery;
        }

        public static QueryExpression GetPluginSteps(string pluginAssemblyName)
        {
            var stepQuery = new QueryExpression(LogicalNames.SdkMessageProcessingStepEntityName);
            stepQuery.ColumnSet = new ColumnSet(true);

            var filterLink = stepQuery.AddLink("sdkmessagefilter", "sdkmessagefilterid", "sdkmessagefilterid", JoinOperator.LeftOuter);
            filterLink.Columns.AddColumns("secondaryobjecttypecode", "primaryobjecttypecode");

            var typeLink = stepQuery.AddLink("plugintype", "plugintypeid", "plugintypeid");

            var assemblyLink = typeLink.AddLink("pluginassembly", "pluginassemblyid", "pluginassemblyid");
            assemblyLink.LinkCriteria.AddCondition("name", ConditionOperator.Equal, pluginAssemblyName);

            return stepQuery;
        }

        public static QueryExpression GetPluginImages(string pluginAssemblyName)
        {
            var imageQuery = new QueryExpression(LogicalNames.SdkMessageProcessingStepImageEntityName);
            imageQuery.Distinct = true;
            imageQuery.ColumnSet = new ColumnSet(true);

            var stepLink = imageQuery.AddLink("sdkmessageprocessingstep", "sdkmessageprocessingstepid", "sdkmessageprocessingstepid");
            stepLink.Columns.AddColumns("sdkmessageid", "description");

            var typeLink = stepLink.AddLink("plugintype", "plugintypeid", "plugintypeid");

            var assemblyLink = typeLink.AddLink("pluginassembly", "pluginassemblyid", "pluginassemblyid");

            var typeAssemblyLink = assemblyLink.AddLink("plugintype", "pluginassemblyid", "pluginassemblyid");
            typeAssemblyLink.LinkCriteria.AddCondition("assemblyname", ConditionOperator.Equal, pluginAssemblyName);

            return imageQuery;
        }

        internal static QueryExpression GetSdkMessageFilters(string pluginAssemblyName)
        {
            var filterQuery = new QueryExpression(LogicalNames.SdkMessageFilterEntityName);
            filterQuery.Distinct = true;
            filterQuery.ColumnSet = new ColumnSet(true);

            var stepLink = filterQuery.AddLink("sdkmessageprocessingstep", "sdkmessagefilterid", "sdkmessagefilterid");

            var typeLink = stepLink.AddLink("plugintype", "plugintypeid", "plugintypeid");

            var assemblyLink = typeLink.AddLink("pluginassembly", "pluginassemblyid", "pluginassemblyid");
            assemblyLink.LinkCriteria.AddCondition("name", ConditionOperator.Equal, pluginAssemblyName);

            return filterQuery;
        }
    }
}
