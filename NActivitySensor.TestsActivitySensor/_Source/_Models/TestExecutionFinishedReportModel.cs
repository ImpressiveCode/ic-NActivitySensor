namespace NActivitySensor
{
    #region Usings
    using Microsoft.VisualStudio.TestWindow.Controller;
using Microsoft.VisualStudio.TestWindow.Data;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    #endregion

    public class TestExecutionFinishedReportModel
    {
        #region Properties
        public int ExecutedTestCount
        {
            get;
            set;
        }

        public double TotalRunTimeMs
        {
            get;
            set;
        }

        public string DominantTaskState
        {
            get;
            set;
        }

        public TestRunRequestStats RunStats
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public TestExecutionFinishedReportModel(TestRunRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            ExecutedTestCount = request.TotalTestCount;
            TotalRunTimeMs = request.TotalRuntime.TotalMilliseconds;
            DominantTaskState = request.DominantTestState.ToString();
            RunStats = request.RunStats;
        }
        #endregion
    }
}
