﻿namespace NActivitySensor.MSSql
{
    #region Usings
    using NActivitySensor.Models;
    using NActivitySensor.MSSql.Models;
    using System;
    using System.Configuration;
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
        private string _ConnectionString;
        #endregion

        #region Constructors
        public MSSqlReporter(IConnectContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _Context = context;
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            if (_HasFailed)
            {
                return;
            }

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
                    throw new ReporterException(exception.Message, exception);
                }
            }
        }
        #endregion

        #region My methods
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
