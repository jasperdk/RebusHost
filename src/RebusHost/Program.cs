using System;
using Ninject;
using Rebus;
using Rebus.Configuration;
using Rebus.Log4Net;
using Rebus.Ninject;
using Rebus.Transports.Msmq;
using Topshelf;
using log4net.Config;

namespace RebusHost
{
    class Program :IHandleMessages<string>
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

            _kernel.Bind<IHandleMessages<string>>().ToConstant(this);

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

        public void Handle(string message)
        {
            Console.WriteLine("w00t!!!: {0}", message);
        }
    }
}
