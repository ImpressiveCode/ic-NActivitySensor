namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class DebuggerExceptionContent
    {
        #region Properties
        public string ExceptionType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Code
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ExceptionAction
        {
            get;
            set;
        }
        #endregion
    }
}
