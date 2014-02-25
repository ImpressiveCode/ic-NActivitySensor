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
            if (message != null)
            {
                Console.WriteLine(message);
            }            
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        public void Log(Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} : {1}", exception.Message, exception.StackTrace));
            }
        }
        #endregion
    }
}
