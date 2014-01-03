using NActivitySensor;
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
                throw new ArgumentNullException("eventName");
            }

            this.Content = ReportSerializationHelper.SerializeToJson(content);
            this.ContentType = content.GetType().Name;
            this.Date = DateTime.Now;
            this.Event = eventName;
        }
        #endregion

        public string Content
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public string Event
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
    }
}
