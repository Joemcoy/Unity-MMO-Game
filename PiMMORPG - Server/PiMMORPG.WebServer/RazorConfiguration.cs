using System;
using System.Linq;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

using tFramework.Factories;

namespace PiMMORPG.WebServer
{
    public class RazorConfiguration : IRazorConfiguration
    {
        string[] assemblies, namespaces;

        public IEnumerable<string> BlacklistedNamespaces
        {
            get
            {
                yield return "Microsoft.VisualBasic";
                yield return "System.Web";
                yield return "System.Timers";
                yield return "System.Net";
                yield return "System.Security.Authentication";
                yield return "System.Windows";
                yield return "System.Media";
                yield return "System.ComponentModel";
                yield return "System.CodeDom";
                yield return "System.Text.RegularExpressions";
                yield return "System.Web";
                yield return "System.Configuration.Internal";
                yield return "System.Configuration.Provider";
                yield return "Microsoft.SqlServer";
                yield return "System.Xml";
                yield return "System.Data";
                yield return "System.Transactions";
                yield return "System.Drawing";
                yield return "System.EnterpriseServices";
                yield return "System.Configuration.Install";
                yield return "System.Collections.Specialized";
                yield return "System.IO.Ports";
                yield return "System.IO.Compression";
				yield return "System.Management";
				yield return "System.Runtime.Serialization.Configuration";
				yield return "System.Runtime.Serialization.Json";
            }
        }

        public RazorConfiguration()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a =>
            {
                var name = a.GetName().Name;
                return !a.IsDynamic && !name.StartsWith("System") && !name.StartsWith("mscorlib") && !name.StartsWith("Microsoft");
            }).ToArray();

            var logger = LoggerFactory.GetLogger(this);

            logger.LogInfo("Caching razor assemblies...");
            this.assemblies = assemblies.Select(a => a.GetName().Name).ToArray();

            logger.LogInfo("Caching razor namespaces...");
            namespaces = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsPublic)).Select(t => t.Namespace).Distinct().ToArray();
            namespaces = namespaces.Where(n => n != null && !BlacklistedNamespaces.Any(o => n.IndexOf(o) > -1)).ToArray();

            //foreach (var name in namespaces)
                //logger.LogSuccess("Cached namespace {0}!", name);

            logger.LogSuccess("Success! Cached {0} assemblies and {1} namespaces!", assemblies.Length, namespaces.Length);
        }

        public IEnumerable<string> GetAssemblyNames()
        {
            foreach (var asm in assemblies)
                yield return asm;
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            foreach (var nms in namespaces)
                yield return nms;
            yield return "IOPath = System.IO.Path";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}