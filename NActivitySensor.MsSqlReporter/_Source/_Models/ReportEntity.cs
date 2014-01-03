using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    [Table("Reports")]
    class ReportEntity : Report
    {
        #region Private variables
        private Report _ReportBase;
        #endregion

        #region Constructors
        public ReportEntity()
        {
            _ReportBase = new Report();
        }

        public ReportEntity(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException("report");
            }

            _ReportBase = report;
        }
        #endregion

        #region Properties
        [Key]
        public int Id
        {
            get;
            set;
        }

        public new string Content
        {
            get
            {
                return _ReportBase.Content;
            }
            set
            {
                _ReportBase.Content = value;
            }
        }

        public new string ContentType
        {
            get
            {
                return _ReportBase.ContentType;
            }
            set
            {
                _ReportBase.ContentType = value;
            }
        }

        public new DateTime Date
        {
            get
            {
                return _ReportBase.Date;
            }
            set
            {
                _ReportBase.Date = value;
            }
        }

        public new string Event
        {
            get
            {
                return _ReportBase.Event;
            }
            set
            {
                _ReportBase.Event = value;
            }
        }
        #endregion
    }
}
