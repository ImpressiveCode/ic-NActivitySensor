namespace NActivitySensor
{
    #region Usings
    using Autofac;
    using NActivitySensor.MSSql;
    using NActivitySensor.OutputWindow;
    using System;
    using System.Configuration;
    #endregion

    /// <summary>
    /// The bootstrapper
    /// </summary>
    public class Bootstrapper : IDisposable
    {
        #region Private variables
        private ILifetimeScope _Scope;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper(object application, object addIn, ILogger logger, Configuration configuration)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (addIn == null)
            {
                throw new ArgumentNullException("addIn");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            var Builder = new ContainerBuilder();

            Builder.Register(g => new DefaultConnectContext(application, addIn, configuration)).As<IConnectContext>();

            // Logger
            Builder.Register(g => logger).As<ILogger>();

            // Reporters
            Builder.RegisterType<MSSqlReporter>().As<IReporter>().WithMetadata("MSSqlReporter", new object());
            Builder.RegisterType<OutputWindowReporter>().As<IReporter>().WithMetadata("OutputWindowReporter", new object()).PreserveExistingDefaults();

            // Activity sensors
            Builder.RegisterType<TestsActivitySensor>().As<IActivitySensor>().WithMetadata("TestActivitySensor", new object());
            Builder.RegisterType<DTEActivitySensor>().As<IActivitySensor>().WithMetadata("DTEActivitySensor", new object()).PreserveExistingDefaults();

            // Serializer
            Builder.RegisterType<JsonReportContentSerializer>().As<IReportContentSerializer>();

            var Container = Builder.Build();

            _Scope = Container.BeginLifetimeScope();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _Scope.Resolve<T>();
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
                if (_Scope != null)
                {
                    _Scope.Dispose();
                }
            }
        }
        #endregion
    }
}
