namespace NActivitySensor
{
    #region Usings
    using System;

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
