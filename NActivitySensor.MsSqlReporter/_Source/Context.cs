using NActivitySensor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    class Context : DbContext
    {
        #region Hacks
        /// <summary>
        /// Make sure "EntityFramework.SqlServer.dll" library will be copied
        /// to bin directory
        /// </summary>
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
