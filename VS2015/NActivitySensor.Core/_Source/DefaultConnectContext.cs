﻿namespace NActivitySensor
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
        private readonly Configuration _Configuration;
        #endregion

        #region Constructors
        public DefaultConnectContext(object application, Configuration configuration)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _Configuration = configuration;
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

        public string GetAppSetting(string key)
        {
            if (CurrentSolutionConfiguration != null && CurrentSolutionConfiguration.AppSettings != null)
            {
                if (CurrentSolutionConfiguration.AppSettings.Settings[key] != null)
                {
                    return CurrentSolutionConfiguration.AppSettings.Settings[key].Value;
                }
            }

            if (DefaultConfiguration.AppSettings != null && DefaultConfiguration.AppSettings.Settings[key] != null)
            {
                return DefaultConfiguration.AppSettings.Settings[key].Value;
            }

            return null;
        }
        #endregion        
    }
}
