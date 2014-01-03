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
    /// <summary>
    /// The bootstrapper
    /// </summary>
    public class BootStrapper
    {
        /// <summary>
        /// Gets the sensors.
        /// </summary>
        /// <value>
        /// The sensors.
        /// </value>
        public IEnumerable<IActivitySensor> Sensors
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapper"/> class.
        /// </summary>
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
