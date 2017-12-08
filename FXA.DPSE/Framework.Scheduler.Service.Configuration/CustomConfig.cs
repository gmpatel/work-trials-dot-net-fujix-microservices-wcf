using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSE.Framework.Scheduler.Service.Configuration.Elements;

namespace FXA.DPSE.Framework.Scheduler.Service.Configuration
{
    public class CustomConfig
    {
        private readonly CustomConfigSection _config;
        private static volatile CustomConfig _instance;

        private static readonly object ConsturctorLock = new object();

        private IList<AssemblyElement> _assemblyElements;
        private IList<Assembly> _assemblies;

        private CustomConfig()
        {
            if (_config == null)
            {
                _config = (CustomConfigSection)ConfigurationManager.GetSection("serviceConfig");
            }
        }

        public static CustomConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (ConsturctorLock)
                    {
                        if (_instance == null) _instance = new CustomConfig();
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
            get { return GetAssemblies(); }
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