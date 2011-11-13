using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Bootstrapping
{
    public class AssemblyPool
    {
        private static readonly List<Assembly> _appAssemblies = new List<Assembly>();
        private static readonly List<Assembly> _otherAssemblies = new List<Assembly>();

        static AssemblyPool()
        {
            var plugins = GetPlugins();
            _appAssemblies = new List<Assembly>(typeof(AssemblyPool).Assembly.ToEnumerable().Concat(plugins));
            AppDomain.CurrentDomain.AssemblyResolve += HandleAssemblyLookup;
        }

        public static IEnumerable<Assembly> ApplicationAssemblies()
        {
            return _appAssemblies;
        }

        private static Assembly HandleAssemblyLookup(object sender, ResolveEventArgs args)
        {
            var asbly = _appAssemblies.Concat(_otherAssemblies).FirstOrDefault(a => a.FullName == args.Name);
            return asbly;
        }

        private static IEnumerable<Assembly> GetPlugins()
        {
            var plugins = new List<Assembly>();
            if (Directory.Exists("plugins"))
            {
                foreach (var f in Directory.GetFiles("plugins", "*.dll"))
                {
                    try
                    {
                        var a = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), f));
                        if (a.FullName.Contains("Thawmadoce"))
                            plugins.Add(a);
                        else
                            _otherAssemblies.Add(a);
                    }
                    catch (Exception x)
                    {
                        Debug.WriteLine("Assembly load failed!");
                        Debug.WriteLine(x.FullOutput());
                    }
                }
            }
            return plugins;
        }
    }
}