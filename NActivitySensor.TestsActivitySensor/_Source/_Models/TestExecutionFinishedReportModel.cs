﻿namespace NActivitySensor
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ms")]
        public double RuntimeTotalMilliseconds
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
            RuntimeTotalMilliseconds = request.TotalRuntime.TotalMilliseconds;
            DominantTaskState = request.DominantTestState.ToString();
            RunStats = request.RunStats;
        }
        #endregion
    }
}
