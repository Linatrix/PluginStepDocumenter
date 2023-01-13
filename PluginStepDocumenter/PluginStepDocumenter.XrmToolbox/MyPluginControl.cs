using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginStepDocumenter.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace PluginStepDocumenter.XrmToolbox
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void loadAssembliesBtn_Click(object sender, EventArgs e)
        {
            ExecuteMethod(GetAssemblies);
        }

        private void GetAssemblies()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting Assemblies",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(StepQueries.GetPluginAssemblies());
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;

                    if (result?.Entities?.Count > 0)
                    {
                        foreach (var assembly in result.Entities)
                        {
                            assemblyComboBox.Items.Add(assembly.GetAttributeValue<string>("name"));
                        }

                        assemblyComboBox.Text = "Select an assembly";
                    }
                }
            });
        }

        private void assemblyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (assemblyComboBox.SelectedIndex != -1)
            {
                string assemblyName = assemblyComboBox.SelectedItem.ToString();

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Generating assembly JSON",
                    Work = (worker, args) =>
                    {
                        args.Result = PluginStepDocumentBuilder.BuildPluginDocumentation(Service, assemblyName);
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        var result = args.Result as string;

                        if (!string.IsNullOrEmpty(result))
                        {
                            jsonTextBox.Text = result;
                        }
                    }
                });
            }
        }
    }
}