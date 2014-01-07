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
        #endregion

        #region Constructors
        public DefaultConnectContext(object application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            _Application = application;
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
        #endregion
    }
}
