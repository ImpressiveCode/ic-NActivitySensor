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
        #region Properties
        object Application
        {
            get;
        }

        object AddIn
        {
            get;
        }

        Configuration DefaultConfiguration
        {
            get;
        }

        Configuration CurrentSolutionConfiguration
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Try to get app setting from current solution configuration or default configuration
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetAppSetting(string key);
        #endregion
    }
}
