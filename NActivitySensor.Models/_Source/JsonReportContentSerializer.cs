namespace NActivitySensor
{
    #region Usings
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
