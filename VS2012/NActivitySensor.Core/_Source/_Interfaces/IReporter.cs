namespace NActivitySensor
{
    #region Usings
    using NActivitySensor.Models;

    #endregion

    public interface IReporter
    {
        #region Methods
        void Report(Report reportModel);
        #endregion
    }
}
