namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    #endregion

    public class FileLogger : ILogger
    {
        #region Private variables
        private string _FilePath;
        private string _ProcessId;
        #endregion

        #region Constructors
        public FileLogger()
        {
            int ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            if (ProcessId < 0 || ProcessId == int.MaxValue)
            {
                ProcessId = 0;
            }
            _FilePath = @"C:\Temp\NActivityLog.txt";
            _ProcessId = ProcessId.ToString("D10", CultureInfo.InvariantCulture);
        }
        #endregion

        #region ILogger methods
        public void Log(string message)
        {
            StringBuilder FullMessage = new StringBuilder();
            FullMessage.Append(_ProcessId);
            FullMessage.Append(" ");
            FullMessage.Append(" ");
             
            FullMessage.Append(message);
            FullMessage.Append(Environment.NewLine);

            if (!string.IsNullOrWhiteSpace(message))
            {
                System.IO.File.AppendAllText(_FilePath, FullMessage.ToString());
            }
        }
        #endregion
    }
}
