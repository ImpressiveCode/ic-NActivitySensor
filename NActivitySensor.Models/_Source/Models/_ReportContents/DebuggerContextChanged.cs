namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class DebuggerContextChanged
    {
        #region Properties
        public string ProcessName
        {
            get;
            set;
        }

        public int ProcessId
        {
            get;
            set;
        }

        public string ProgramName
        {
            get;
            set;
        }

        public int ProgramProcessId
        {
            get;
            set;
        }

        public string ThreadName
        {
            get;
            set;
        }

        public int ThreadId
        {
            get;
            set;
        }

        public string StackFrameFunctionName
        {
            get;
            set;
        }
        #endregion
    }
}
