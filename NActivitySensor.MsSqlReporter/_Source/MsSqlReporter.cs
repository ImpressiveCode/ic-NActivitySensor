using NActivitySensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public class MsSqlReporter : IReporter
    {
        #region Private variables
        private ILogger _Logger;
        #endregion

        #region Constructors
        public MsSqlReporter(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _Logger = logger;
        }
        #endregion

        #region IReporter methods
        public void Report(Report report)
        {
            try
            {
                var ReportEntity = new ReportEntity(report);

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
