namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class SensorException : Exception
    {
        #region Constructors
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
        #endregion
    }
}
