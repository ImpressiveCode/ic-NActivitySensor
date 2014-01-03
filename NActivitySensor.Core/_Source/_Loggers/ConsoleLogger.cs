namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class ConsoleLogger : ILogger
    {
        #region ILogger methods
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
        #endregion
    }
}
