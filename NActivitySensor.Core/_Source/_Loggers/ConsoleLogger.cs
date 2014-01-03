using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
