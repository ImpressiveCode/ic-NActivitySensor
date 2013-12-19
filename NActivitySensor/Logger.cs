using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace NActivitySensor
{
    internal class Logger
    {
        private string _FilePath;
        private string _ProcessId;
        
        public Logger(int ProcessId)
        {
            if (ProcessId < 0 || ProcessId == int.MaxValue)
            {
                ProcessId = 0;
            }
            _FilePath = @"C:\Temp\NActivityLog.txt";
            _ProcessId = ProcessId.ToString("D10");
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
