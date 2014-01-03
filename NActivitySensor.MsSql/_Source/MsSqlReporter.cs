namespace NActivitySensor.MSSql
{
    #region Usings
    using NActivitySensor.Models;
    using NActivitySensor.MSSql.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class MSSqlReporter : IReporter
    {
        #region Private variables
        #endregion

        #region Constructors
        public MSSqlReporter()
        {
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
