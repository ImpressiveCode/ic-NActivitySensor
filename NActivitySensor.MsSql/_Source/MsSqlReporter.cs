using NActivitySensor.Models;
using NActivitySensor.MSSql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor.MSSql
{
    public class MSSqlReporter : IReporter
    {
        #region Private variables
        private ILogger _Logger;
        #endregion

        #region Constructors
        public MSSqlReporter(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _Logger = logger;
        }
        #endregion

        #region IReporter methods
        public void Report(Report reportModel)
        {
            try
            {
                var ReportEntity = new ReportEntity(reportModel);

                using (Context Context = new Context())
                {
                    Context.Reports.Add(ReportEntity);
                    Context.SaveChanges();
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
