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

        public Logger()
        {
            _FilePath = @"C:\Temp\NActivityLog.txt";    
        }

        public void Log(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                System.IO.File.AppendAllText(_FilePath, message + Environment.NewLine);
            }
        }
    }
}
