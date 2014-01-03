using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor.Models
{
    public class Report
    {
        #region Constructors
        public Report()
        {

        }

        public Report(object content, string eventName)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("rEvent");
            }

            this.Content = ReportSerializationHelper.SerializeToJson(content);
            this.ContentType = content.GetType().Name;
            this.Date = DateTime.Now;
            this.Event = eventName;
        }
        #endregion

        public virtual string Content
        {
            get;
            set;
        }

        public virtual string ContentType
        {
            get;
            set;
        }

        public virtual string Event
        {
            get;
            set;
        }

        public virtual DateTime Date
        {
            get;
            set;
        }
    }
}
