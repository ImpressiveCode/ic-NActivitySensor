using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NActivitySensor
{
    [Serializable]
    public class SensorException : Exception
    {
        public SensorException()
            : base()
        {
        }

        public SensorException(string message)
            : base(message)
        {
        }

        public SensorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected SensorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
