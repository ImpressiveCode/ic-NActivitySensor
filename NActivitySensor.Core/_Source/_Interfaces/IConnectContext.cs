namespace NActivitySensor
{
    #region Usings
    using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
    #endregion

    public interface IConnectContext
    {
        #region Methods
        object Application
        {
            get;
        }

        object AddIn
        {
            get;
        }

        Configuration Configuration
        {
            get;
        }
        #endregion
    }
}
