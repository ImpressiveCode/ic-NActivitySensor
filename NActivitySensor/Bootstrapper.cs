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

            var Container = Builder.Build();

            Scope = Container.BeginLifetimeScope();
        }
        #endregion

        #region IDisposable methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
