using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Ninject;
using Rebus;
using Rebus.Configuration;
using Rebus.Log4Net;
using Rebus.Ninject;
using Rebus.Transports.Msmq;
using RebusHost.Configuration;
using Topshelf;
using log4net.Config;

namespace RebusHost
{
    class Program
    {
        private IBus _bus;
        private StandardKernel _kernel;

        static void Main(string[] args)
        {
            var rebusHostSection =
               ConfigurationManager.GetSection("rebusHost") as RebusHostConfigurationSection;

            XmlConfigurator.Configure();

            HostFactory
                .Run(c =>
                            {
                                c.SetServiceName(rebusHostSection.ServiceName);
                                c.SetDisplayName(rebusHostSection.DisplayName);
                                c.Service<Program>(p =>
                                                    {
                                                        p.ConstructUsing(() => new Program());
                                                        p.WhenStarted(s => s.Start());
                                                        p.WhenStopped(s => s.Stop());
                                                    });
                            });
        }

        void Start()
        {
            CreateKernel();

            _bus = Configure.With(new NinjectContainerAdapter(_kernel))
                .Logging(l => l.Log4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .Serialization(s => s.UseJsonSerializer())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .CreateBus()
                .Start();

            _bus.Send("HELLLLLO WORLD!!!1");

        }

        private void CreateKernel()
        {
            var rebusHostSection =
                ConfigurationManager.GetSection("rebusHost") as RebusHostConfigurationSection;

            _kernel = new StandardKernel();

            foreach (HandlerPathConfigElement handlerPath in rebusHostSection.HandlerPaths)
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

                var referencedPaths = Directory.GetFiles(handlerPath.Path, "*.dll");
                var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
                toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
                _kernel.Load(loadedAssemblies);
            }
        }

        void Stop()
        {
            if (_bus != null)
                _bus.Dispose();
            _bus = null;

            if (_kernel != null)
                _kernel.Dispose();
            _kernel = null;
        }
    }
}
