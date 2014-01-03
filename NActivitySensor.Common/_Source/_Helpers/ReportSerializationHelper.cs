namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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
