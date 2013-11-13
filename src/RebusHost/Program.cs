using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Ninject;
using Rebus;
using Rebus.Configuration;
using Rebus.Log4Net;
using Rebus.Ninject;
using Rebus.Transports.Msmq;
using RebusHost.HandlerSample;
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
            XmlConfigurator.Configure();

            HostFactory
                .Run(c =>
                            {
                                c.SetServiceName("RebusTest");
                                c.SetDisplayName("RebusTest");
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
            _kernel = new StandardKernel();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
            _kernel.Load(loadedAssemblies);

            _bus = Configure.With(new NinjectContainerAdapter(_kernel))
                .Logging(l => l.Log4Net())
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .Serialization(s => s.UseJsonSerializer())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .CreateBus()
                .Start();

            _bus.Send("HELLLLLO WORLD!!!1");

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
