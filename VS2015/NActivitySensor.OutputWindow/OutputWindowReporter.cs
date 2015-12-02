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
        private readonly List<Report> _ReportsToShow;

        private Window _Window;
        private OutputWindow _OutputWindow;
        private OutputWindowPane _OutputWindowPane;
        private bool _IsInitialized;

        private object _Lock = new object();
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
            _ReportsToShow = new List<Models.Report>();
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            _ReportsToShow.Add(reportModel);

            if (!_IsInitialized)
            {
                _IsInitialized = MyTryInitialize();
            }

            if (!_IsInitialized)
            {
                return;
            }

            try
            {
                foreach (var loopReport in _ReportsToShow)
                {
                    string Format = "[{0}] [{1}] {2}" + Environment.NewLine;
                    string OutputMessage = String.Format(Format, loopReport.Date, loopReport.Event, loopReport.Content);
                    _OutputWindowPane.OutputString(OutputMessage);
                }
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion

        #region My methods
        private bool MyTryInitialize()
        {
            try
            {
                if (_Application.Windows == null)
                {
                    return false;
                }

                _Window = _Application.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);

                _OutputWindow = (OutputWindow)_Window.Object;
                _OutputWindowPane = _OutputWindow.OutputWindowPanes.Add("NActivitySensor");

                if (_OutputWindowPane == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion
    }
}
