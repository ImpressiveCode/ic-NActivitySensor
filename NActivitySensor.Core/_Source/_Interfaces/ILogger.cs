using System;
namespace NActivitySensor
{
    #region Usings

    #endregion

    public interface ILogger
    {
        #region Methods
        void Log(string message);

        void Log(Exception exception);
        #endregion
    }
}
