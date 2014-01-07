namespace NActivitySensor.Models
{
    #region Usings
    using NActivitySensor;
    using System;

    #endregion
    public class Report
    {
        #region Constructors
        public Report()
        {

        }

        public Report(object content, string eventName, int? processId, string solution, IReportContentSerializer reportContentSerializer)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }

            if (reportContentSerializer == null)
            {
                throw new ArgumentNullException("reportContentSerializer");
            }

            this.Content = reportContentSerializer.Serialize(content);
            this.ContentType = content.GetType().Name;
            this.Date = DateTime.Now;
            this.Event = eventName;
            this.ProcessId = processId;
            this.Solution = solution;
            this.EnvironmentUserName = Environment.UserName;
        }
        #endregion

        #region Properties
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

        public int? ProcessId
        {
            get;
            set;
        }

        public string Solution
        {
            get;
            set;
        }

        public string EnvironmentUserName
        {
            get;
            set;
        }
        #endregion
    }
}
