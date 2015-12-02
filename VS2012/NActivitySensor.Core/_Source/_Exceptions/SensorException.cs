namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class ReporterException : Exception
    {
        #region Constructors
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

        protected ReporterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
