namespace NActivitySensor
{
    #region Usings
    using Microsoft.VisualStudio.TestWindow.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public class TestResultModel
    {
        #region Constructors
        public TestResultModel(IResult anotherResult)
        {
            if (anotherResult == null)
            {
                throw new ArgumentNullException("anotherResult");
            }

            DisplayName = anotherResult.DisplayName;
            Duration = anotherResult.Duration;
            ErrorMessage = anotherResult.ErrorMessage;
            ErrorStackTrace = anotherResult.ErrorStackTrace;
            Outcome = anotherResult.Outcome;
            ReceivedOrder = anotherResult.ReceivedOrder;
            RunId = anotherResult.RunId;
            StandardError = anotherResult.StandardError;
            StandardOutput = anotherResult.StandardOutput;
            State = anotherResult.State;
        }
        #endregion

        #region IResult methods
        public string DisplayName
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string ErrorStackTrace
        {
            get;
            set;
        }

        public short Outcome
        {
            get;
            set;
        }

        public long ReceivedOrder
        {
            get;
            set;
        }

        public short RunId
        {
            get;
            set;
        }

        public string StandardError
        {
            get;
            set;
        }

        public string StandardOutput
        {
            get;
            set;
        }

        public TestState State
        {
            get;
            set;
        }
        #endregion
    }
}
