using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NActivitySensor
{
    class ReporterException : Exception
    {
        public ReporterException()
            : base()
        {
        }

        public ReporterException(string message)
            : base(message)
        {
        }

        public ReporterException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public ReporterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
