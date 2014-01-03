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
using NActivitySensor.MsSql;

namespace NActivitySensor
{
    /// <summary>
    /// The bootstrapper
    /// </summary>
    public class BootStrapper
    {
        #region Properties
        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public ILifetimeScope Scope
        {
            get;
            private set;
        }
        #endregion

        #region Constructors
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

            Scope = Container.BeginLifetimeScope();
        }
        #endregion

    }
}
