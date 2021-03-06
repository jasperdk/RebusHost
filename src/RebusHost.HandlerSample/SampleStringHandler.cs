﻿using System;
using Newtonsoft.Json;
using Ninject.Modules;
using Rebus;

namespace RebusHost.HandlerSample
{
    public class SampleStringHandler :IHandleMessages<string>
    {
        public class HandlerSampleNinjectModule:NinjectModule
        {
            public override void Load()
            {
                Bind<IHandleMessages<string>>().To<SampleStringHandler>();
            }
        }

        public void Handle(string message)
        {
            Console.WriteLine("w00t!!!: {0}", message);

            string json = JsonConvert.SerializeObject(message);

            Console.WriteLine("Testing referenced dll: {0}", json);
        }
    }
}
