namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    #endregion

    public class FileLogger : ILogger
    {
        #region Private constants
        private const string _ConstLogFileName = "NActivityLog.txt";
        #endregion

        #region Private variables
        private string _FilePath;
        private string _ProcessId;
        #endregion

        #region Constructors
        public FileLogger()
        {
            var CurrentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
            var DirectoryName = System.IO.Path.GetDirectoryName(CurrentAssemblyLocation);

            int ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            if (ProcessId < 0 || ProcessId == int.MaxValue)
            {
                ProcessId = 0;
            }

            _FilePath = String.Format(CultureInfo.InvariantCulture, "{0}\\{1}", DirectoryName, _ConstLogFileName);
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

        public void Log(Exception exception)
        {
            Log(string.Format("{0} {1}", exception.Message, exception.StackTrace));
        }
        #endregion
    }
}
