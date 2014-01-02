using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NActivitySensor.MsSqlReporter
{
    class Context : DbContext
    {
        #region Constructors
        public Context()
            : base()
        {

        }
        #endregion

        #region Properties
        public DbSet<ReportEntity> Reports
        {
            get;
            set;
        }
        #endregion

    }
}
