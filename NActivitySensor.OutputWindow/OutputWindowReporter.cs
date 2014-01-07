namespace NActivitySensor.OutputWindow
{
    #region Usings
    using EnvDTE;
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
        private readonly Window _Window;
        private readonly OutputWindow _OutputWindow;
        private readonly OutputWindowPane _OutputWindowPane;
        // private 
        #endregion

        #region Constructors
        public OutputWindowReporter(IConnectContext connectContext)
        {
            if (connectContext == null)
            {
                throw new ArgumentNullException("connectContext");
            }

            try
            {
                _ConnectContext = connectContext;

                _Application = (DTE2)_ConnectContext.Application;

                _Window = _Application.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
                _OutputWindow = (OutputWindow)_Window.Object;
                _OutputWindowPane = _OutputWindow.OutputWindowPanes.Add("NActivitySensor");
                _OutputWindowPane.Activate();
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            try
            {
                if (_OutputWindowPane != null)
                {
                    string Format = "[{0}] [{1}] {2}\n";
                    string OutputMessage = String.Format(Format, reportModel.Date, reportModel.Event, reportModel.Content);
                    _OutputWindowPane.OutputString(OutputMessage);
                }
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion
    }
}
