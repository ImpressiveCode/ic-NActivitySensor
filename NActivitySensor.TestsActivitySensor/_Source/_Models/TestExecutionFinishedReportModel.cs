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

        public Dictionary<string, int> RunStats
        {
            get;
            set;
        }

        public IList<TestModel> Tests
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public TestExecutionFinishedReportModel()
        {
        }

        public TestExecutionFinishedReportModel(TestRunRequest request, IEnumerable<ITest> tests)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (tests == null)
            {
                throw new ArgumentNullException("tests");
            }

            // Basic information
            ExecutedTestCount = request.TotalTestCount;
            RuntimeTotalMilliseconds = request.TotalRuntime.TotalMilliseconds;
            DominantTaskState = request.DominantTestState.ToString();

            // Convert stats
            RunStats = new Dictionary<string, int>();
            if (request.RunStats != null && request.RunStats.Stats != null)
            {
                foreach (var LoopStat in request.RunStats.Stats)
                {
                    RunStats.Add(LoopStat.Key.ToString(), LoopStat.Value);
                }
            }

            // Convert tests
            Tests = new List<TestModel>();
            foreach (var LoopTest in tests)
            {
                Tests.Add(new TestModel(LoopTest));
            }
        }
        #endregion
    }
}
