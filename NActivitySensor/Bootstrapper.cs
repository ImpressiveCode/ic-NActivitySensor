namespace NActivitySensor
{
    #region Usings
    using Autofac;
    using NActivitySensor.MSSql;
    using System;
    #endregion

    /// <summary>
    /// The bootstrapper
    /// </summary>
    public class Bootstrapper : IDisposable
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
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            var Builder = new ContainerBuilder();

            Builder.RegisterType<FileLogger>().As<ILogger>();
            Builder.RegisterType<MSSqlReporter>().As<IReporter>();
            Builder.RegisterType<DTEActivitySensor>().As<IActivitySensor>();
            Builder.RegisterType<JsonReportContentSerializer>().As<IReportContentSerializer>();

            var Container = Builder.Build();

            Scope = Container.BeginLifetimeScope();
        }
        #endregion

        #region IDisposable methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Scope != null)
                {
                    Scope.Dispose();
                }
            }
        }
        #endregion
    }
}
