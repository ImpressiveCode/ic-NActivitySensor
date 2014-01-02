using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NActivitySensor.MsSqlReporter
{
    [Table("Reports")]
    class ReportEntity : Report
    {
        #region Properties
        [Key]
        public int Id
        {
            get;
            set;
        }
        #endregion
    }
}
