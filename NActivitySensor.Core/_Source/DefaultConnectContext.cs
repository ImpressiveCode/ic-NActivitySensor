namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class DefaultConnectContext : IConnectContext
    {
        #region Private variables
        private readonly object _Application;
        private readonly object _AddIn;
        #endregion

        #region Constructors
        public DefaultConnectContext(object application, object addIn)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (addIn == null)
            {
                throw new ArgumentNullException("addIn");
            }

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
        #endregion
    }
}
