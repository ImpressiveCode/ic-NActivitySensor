namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    #endregion

    public class DefaultConnectContext : IConnectContext
    {
        #region Private variables
        private readonly object _Application;
        private readonly object _AddIn;
        private readonly Configuration _Configuration;
        #endregion

        #region Constructors
        public DefaultConnectContext(object application, object addIn, Configuration configuration)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (addIn == null)
            {
                throw new ArgumentNullException("addIn");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _Configuration = configuration;
            _Application = application;
            _AddIn = addIn;
        }
        #endregion

        #region IConnectContext properties
        public object Application
        {
            get
            {
                return _Application;
            }
        }

        public object AddIn
        {
            get
            {
                return _AddIn;
            }
        }

        public Configuration DefaultConfiguration
        {
            get 
            { 
                return _Configuration; 
            }
        }

        public Configuration CurrentSolutionConfiguration
        {
            get;
            set;
        }
        #endregion
    }
}
