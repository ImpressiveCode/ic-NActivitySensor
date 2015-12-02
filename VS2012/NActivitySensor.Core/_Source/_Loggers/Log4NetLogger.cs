namespace NActivitySensor
{
    #region Usings
    using log4net;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    #endregion

    public class Log4NetLogger : ILogger
    {
        #region Private variables
        private readonly ILog _Log;
        #endregion

        #region Constructors
        public Log4NetLogger(IConnectContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            try
            {
                if (context.DefaultConfiguration != null && context.DefaultConfiguration.FilePath != null)
                {
                    var ConfigurationFile = context.DefaultConfiguration.FilePath;
                    FileInfo ConfigurationFileInfo = new FileInfo(ConfigurationFile);
                    var Configuration = log4net.Config.XmlConfigurator.Configure(ConfigurationFileInfo);                    
                }

                _Log = LogManager.GetLogger("NActivitySensorLog4NetLogger");
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ILogger methods
        public void Log(string message)
        {
            if (message != null)
            {
                _Log.Error(message);
            }            
        }

        public void Log(Exception exception)
        {
            if (exception != null && exception.Message != null)
            {
                _Log.Error(exception.Message, exception);
            }
        }
        #endregion
    }
}
