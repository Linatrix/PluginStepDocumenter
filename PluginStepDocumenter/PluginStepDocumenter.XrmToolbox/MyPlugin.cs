using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace PluginStepDocumenter.XrmToolbox
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Plugin Step Documenter"),
        ExportMetadata("Description", "Creates JSON documentation of plugin steps for storage in source control or comparison with other environments"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABmJLR0QA/wD/AP+gvaeTAAAC5klEQVRYhe3WT4hVdRjG8c97rw2NTjqDf8qRNDGtLNCxaBFITVCLDMtqU2nSH6SNQe2CIiKMghCjRS4kJFxNRIug2heBcJ0zjWO0qGBokzGEEmNq43lbzMUmmXvv3PlTBL1wOZz7vO/zfM+P3/lx+L9mWVmzOAftnKtPZdaTYXfyyb8GkOycy/ycAPIbS4R75xo+awClh9DZrCWH9Gdhx4IAZHqyqT6gmqXDmZ6Zd4CsWY37mzZttBubZuLXNkBZtQ+LGulZ6M50YKZ+bQFkzVWR9jXrKTmINU19UiW/ntxD7a1AeAS9DY0LD0d6umn4KR1Z+KLsdLQtgEyR4eWG+qDNmZOmzTzKC47gvkhL2wIw5DFsmdb4pGuTz7GsaXjh3WDP1P9nBJCpkum1Rnr5h0NY23B+QLUsHAn2X6k13M1/q0F7hVsbyUF3w/BUySEfRdo1nd5yBfK45RnenhHodHXC1RqEzwigXOQdrJw1QItqCpCFeyLsXajwpgA5rCfTB4h/HCAHVHPCMaxvMX+2fu2YV4DyRgfxQIOZxJeRng82Z81a3DlvADnoleCFaXpPZ3ozwk3R525huOSlrDiBrtkCXD4H6ifVgXTFcRt+DN7S5cMYt6JMz0XhWVw/ZXP8gp8xXv+dQUb6WNUa2QIgT+nKwtHg0SnaWPCGDodNWF/+5lCwURqJ8DpGVf3kV6PR7/xU00zhpG6XrMj0fssVKM97L+Kv8AzHKpe8GHcYy8INWFo5a3/0m8hB64TbsLWc8KBuq8tCL3qlVejKolnkNAARrqvfn4n0VGWbT6f0dEvbc5lXy0Hbc/JefU6z5Z0xAM5JI8GuuN33WbOhDHsiPJ55+dNqDD/gNMZz8hX8Hefq+hJ0xuSpuRIbsKoVQEB+5Ro9LrhoXckT0i0VvpVGhO9UjcYW4+0+XQ7rcdEmVTeXpb4Id6Gv/uCfVbZN+WrOAdU8bnm7IW1D1SzOwo4sbF3orP9G/QksL//VEnr34AAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Black"),
        ExportMetadata("PrimaryFontColor", "White"),
        ExportMetadata("SecondaryFontColor", "DarkGray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}