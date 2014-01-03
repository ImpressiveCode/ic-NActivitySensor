using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public class FileLogger : ILogger
    {
        private string _FilePath;
        private string _ProcessId;
        
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
    }
}
