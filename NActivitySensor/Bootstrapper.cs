using Autofac;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Autofac.Integration.Mef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NActivitySensor.ActivitySensors;
using NActivitySensor.Loggers;

namespace NActivitySensor
{
    public class BootStrapper
    {
        public IEnumerable<IActivitySensor> Sensors
        {
            get;
            private set;
        }

        public BootStrapper()
        {
            var Builder = new ContainerBuilder();

            Builder.RegisterType<FileLogger>().As<ILogger>();
            Builder.RegisterType<MsSqlReporter>().As<IReporter>();
            Builder.RegisterType<BuildActivitySensor>().As<IActivitySensor>();

            var Container = Builder.Build();

            var Scope = Container.BeginLifetimeScope();

            Sensors = Scope.Resolve<IEnumerable<IActivitySensor>>();
        }
    }
}
