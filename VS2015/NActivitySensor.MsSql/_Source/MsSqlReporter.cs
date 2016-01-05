namespace NActivitySensor.MSSql
{
    #region Usings
    using NActivitySensor.Models;
    using NActivitySensor.MSSql.Models;
    using System;
    using System.Collections.Generic;
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
        private Queue<Report> _ReportQueue = new Queue<Report>();
        private int _AddToDbTimerIntervalInSeconds = 10;
        private System.Timers.Timer _AddToDbTimer;
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

            this._Context = context;
            this._Logger = logger;
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            if (this._HasFailed)
            {
                return;
            }
            
            this.EnqueueReport(reportModel);
        }

        #endregion

        #region My methods
        private void EnqueueReport(Report reportModel)
        {
            if (_ReportQueue.Count < 1)
            {
                // start task with timer
                this.SetAddToDbTimer();
            }
            _ReportQueue.Enqueue(reportModel);
        }

        private Task SetAddToDbTimer()
        {
            return Task.Factory.StartNew(() => 
            {
                if (_AddToDbTimer == null)
                {
                    _AddToDbTimer = new System.Timers.Timer();
                    _AddToDbTimer.Elapsed += AddToDbTimerElapsed;
                }

                _AddToDbTimer.Interval = _AddToDbTimerIntervalInSeconds * 1000;
                _AddToDbTimer.Enabled = true;
            });
        }

        private void AddToDbTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this._Lock)
            {

                try
                {
                    var ReportEntities = new List<ReportEntity>();
                    while (_ReportQueue.Count > 0)
                    {
                        ReportEntities.Add(new ReportEntity(_ReportQueue.Peek()));
                        _ReportQueue.Dequeue();
                    }

                    this._ConnectionString = this._Context.GetAppSetting(_ConstAppSettingsConnectionStringKey);

                    using (Context DatabaseContext = this.MyCreateDatabaseContext())
                    {
                        foreach (var entity in ReportEntities)
                        {
                            DatabaseContext.Reports.Add(entity);
                        }
                        DatabaseContext.SaveChanges();
                    }

                    ReportEntities.Clear();
                }
                catch (Exception exception)
                {
                    this._HasFailed = true;

                    this._Logger.Log(exception);
                }
            }
        }

        private Context MyCreateDatabaseContext()
        {
            Context DatabaseContext = null;
            if (String.IsNullOrEmpty(this._ConnectionString))
            {
                DatabaseContext = new Context();
            }
            else
            {
                DatabaseContext = new Context(this._ConnectionString);
            }
            return DatabaseContext;
        }
        #endregion
    }
}
