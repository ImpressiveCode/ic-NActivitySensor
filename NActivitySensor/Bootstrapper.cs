using Autofac;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Autofac.Integration.Mef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NActivitySensor
{
    public class BootStrapper
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

        public IEnumerable<IActivitySensor> Sensors
        {
            get;
            private set;
        }

        public BootStrapper()
        {
            var Builder = new ContainerBuilder();
            var AssemblyDirectory = Assembly.GetExecutingAssembly().GetCurrentAssemblyDirectory();
            var PluginsCatalog = new DirectoryCatalog(AssemblyDirectory);


            Builder.RegisterType<FileLogger>().As<ILogger>();
            Builder.RegisterType<MsSqlReporter>().As<IReporter>();
            Builder.RegisterType<BuildActivitySensor>().As<IActivitySensor>();

            Container = Builder.Build();

            Scope = Container.BeginLifetimeScope();
            Sensors = Scope.Resolve<IEnumerable<IActivitySensor>>();
        }
    }
}
