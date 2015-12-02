namespace NActivitySensor.MSSql
{
    #region Usings
    using NActivitySensor.Models;
    using NActivitySensor.MSSql.Models;
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    public class MSSqlReporter : IReporter
    {
        #region Private constants
        private const string _ConstAppSettingsConnectionStringKey = "NActivitySensor.MSSql.ConnectionString";
        private bool _HasFailed = false;
        #endregion

        #region Private variables
        private object _Lock = new object();
        private readonly IConnectContext _Context;
        private readonly ILogger _Logger;
        private string _ConnectionString;
        #endregion

        #region Constructors
        public MSSqlReporter(IConnectContext context, ILogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _Context = context;
            _Logger = logger;
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            if (_HasFailed)
            {
                return;
            }

            MyReportAsync(reportModel);
        }
        #endregion

        #region My methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private Task MyReportAsync(Report reportModel)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_Lock)
                {
                    try
                    {
                        var ReportEntity = new ReportEntity(reportModel);

                        _ConnectionString = _Context.GetAppSetting(_ConstAppSettingsConnectionStringKey);

                        using (Context DatabaseContext = MyCreateDatabaseContext())
                        {
                            DatabaseContext.Reports.Add(ReportEntity);
                            DatabaseContext.SaveChanges();
                        }
                    }
                    catch (Exception exception)
                    {
                        _HasFailed = true;

                        _Logger.Log(exception);
                    } 
                }
            });
        }

        private Context MyCreateDatabaseContext()
        {
            Context DatabaseContext = null;
            if (String.IsNullOrEmpty(_ConnectionString))
            {
                DatabaseContext = new Context();
            }
            else
            {
                DatabaseContext = new Context(_ConnectionString);
            }
            return DatabaseContext;
        }
        #endregion
    }
}
