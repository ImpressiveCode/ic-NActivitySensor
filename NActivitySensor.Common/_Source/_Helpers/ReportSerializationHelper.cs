namespace NActivitySensor
{
    #region Usings
    using Newtonsoft.Json;
    #endregion

    public static class ReportSerializationHelper
    {
        #region Methods
        public static string SerializeToJson(object input)
        {
            return JsonConvert.SerializeObject(input);
        }
        #endregion
    }
}
