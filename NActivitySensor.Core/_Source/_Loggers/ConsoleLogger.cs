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

        public void Log(Exception exception)
        {
            Console.WriteLine(string.Format("{0} : {1}", exception.Message, exception.StackTrace));
        }
        #endregion
    }
}
