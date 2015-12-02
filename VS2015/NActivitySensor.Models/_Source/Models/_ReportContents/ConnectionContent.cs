namespace NActivitySensor.Models
{
    #region Usings
    using System.Collections.Generic;

    #endregion

    public class ConnectionContent
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }

        public string ActiveWindow
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public string Mode
        {
            get;
            set;
        }

        public string MainWindow
        {
            get;
            set;
        }

        public int LocaleId
        {
            get;
            set;
        }

        public string Edition
        {
            get;
            set;
        }

        public IEnumerable<string> Windows
        {
            get;
            set;
        }
        #endregion
    }
}
