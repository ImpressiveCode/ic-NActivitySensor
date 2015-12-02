namespace NActivitySensor
{
    #region Usings

    #endregion

    public interface IReportContentSerializer
    {
        #region Methods
        string Serialize(object content);
        #endregion
    }
}
