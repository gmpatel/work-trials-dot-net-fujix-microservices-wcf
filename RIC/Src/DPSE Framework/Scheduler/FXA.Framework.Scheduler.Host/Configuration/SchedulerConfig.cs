using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using FXA.Framework.Scheduler.Host.Configuration.Elements;

namespace FXA.Framework.Scheduler.Host.Configuration
{
    public class SchedulerConfig
    {
        private readonly SchedulerConfigSection _config;
        private static volatile SchedulerConfig _instance;
        private static readonly object SyncRoot = new object();

        private IList<AssemblyElement> _assemblyElements;
        private IList<Assembly> _assemblies;

        private SchedulerConfig()
        {
            if (_config == null)
            {
                _config = (SchedulerConfigSection)ConfigurationManager.GetSection("serviceConfig");
            }
        }

        public static SchedulerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null) 
                            _instance = new SchedulerConfig();
                    }
                }

                return _instance;    
            }
        }

        public IList<AssemblyElement> AssemblyElements
        {
            get
            {
                return GetAssemblyElements();    
            }
        }

        public IList<AssemblyElement> GetAssemblyElements()
        {
            if (_assemblyElements == null)
            {
                _assemblyElements = new List<AssemblyElement>();

                foreach (var assembly in _config.Assemblies)
                {
                    _assemblyElements.Add((AssemblyElement)assembly);
                }
            }

            return _assemblyElements;
        }

        public IList<Assembly> Assemblies
        {
            get
            {
                return GetAssemblies();
            }
        }

        public IList<Assembly> GetAssemblies()
        {
            if (_assemblies == null)
            {
                _assemblies = new List<Assembly>();

                foreach (var assembly in _config.Assemblies)
                {
                    try
                    {
                        var asm = Assembly.Load(((AssemblyElement)assembly).Name);
                        _assemblies.Add(asm);
                    }
                    catch (Exception exception)
                    {
                        // Handelled - Do not add assembly to the Assemblies collection if not available
                    }
                }
            }

            return _assemblies;
        }
    }
}