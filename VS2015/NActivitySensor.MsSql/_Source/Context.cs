namespace NActivitySensor.MSSql
{
    #region Usings
    using NActivitySensor.MSSql.Models;
    using System;
    using System.Data.Entity;

    #endregion

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
            this.Database.CreateIfNotExists();
        }

        public Context(string connectionString)
            : base(connectionString)
        {
            this.Database.CreateIfNotExists();
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
