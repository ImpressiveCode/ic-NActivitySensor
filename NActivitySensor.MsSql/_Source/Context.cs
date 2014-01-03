using NActivitySensor.Models;
using NActivitySensor.MSSql.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NActivitySensor.MSSql
{
    public class Context : DbContext
    {
        #region Hacks
        /// <summary>
        /// Make sure "EntityFramework.SqlServer.dll" library will be copied
        /// to bin directory
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Type _Hack = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        #endregion

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
