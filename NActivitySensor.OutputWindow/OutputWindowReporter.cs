namespace NActivitySensor.OutputWindow
{
    #region Usings
    using EnvDTE80;
    using NActivitySensor.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public class OutputWindowReporter : IReporter
    {
        #region Private variablers
        private readonly IConnectContext _ConnectContext;
        private readonly DTE2 _Application;
        #endregion

        #region Constructors
        public OutputWindowReporter(IConnectContext connectContext)
        {
            if (connectContext == null)
            {
                throw new ArgumentNullException("connectContext");
            }

            _ConnectContext = connectContext;

            _Application = (DTE2)_ConnectContext.Application;
        }
        #endregion
        #region IReporter methods
        public void Report(Report reportModel)
        {

        }
        #endregion
    }
}
