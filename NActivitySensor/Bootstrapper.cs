using Autofac;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Autofac.Integration.Mef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public class Bootstrapper
    {
        public IContainer Container
        {
            get;
            private set;
        }

        public ILifetimeScope Scope
        {
            get;
            private set;
        }

        public Bootstrapper()
        {
            var Builder = new ContainerBuilder();
            var PluginsCatalog = new DirectoryCatalog(".");

            // Register logger
            Builder.RegisterType<FileLogger>().Exported(g => g.As<ILogger>().WithMetadata("FileLogger", new object()));

            // Register plugins
            Builder.RegisterComposablePartCatalog(PluginsCatalog);

            Container = Builder.Build();

            // TODO
            var Scope = Container.BeginLifetimeScope();
        }
    }
}
