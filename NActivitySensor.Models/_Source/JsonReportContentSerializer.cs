namespace NActivitySensor
{
    #region Usings
    using Newtonsoft.Json;

    #endregion

    public class JsonReportContentSerializer : IReportContentSerializer
    {
        #region IReportContentSerializer
        public string Serialize(object content)
        {
            return JsonConvert.SerializeObject(content);
        }
        #endregion
    }
}
