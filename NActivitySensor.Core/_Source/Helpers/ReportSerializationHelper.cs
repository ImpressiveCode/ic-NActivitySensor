using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NActivitySensor.Helpers
{
    public static class ReportSerializationHelper
    {
        public static string SerializeToJson(object input)
        {
            return JsonConvert.SerializeObject(input);            
        }
    }
}
