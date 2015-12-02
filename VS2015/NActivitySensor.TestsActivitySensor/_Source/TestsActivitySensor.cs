namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel.Composition;
    using NActivitySensor.Models;
    using Microsoft.VisualStudio.TestWindow.Extensibility;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TestWindow.Controller;
    using System.Threading;
    #endregion

    /// <summary>
    /// C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow
    /// </summary>
    public class TestsActivitySensor : BaseActivitySensor, IDisposable
    {
        #region Private variables
        private ServiceProvider _ServiceProvider;
        private IComponentModel _ComponentModel;
        private IOperationState _OperationState;
        private ITestsService _TestsService;

        private readonly IEnumerable<IReporter> _Reporters;
        private readonly IReportContentSerializer _ReportContentSerializer;

        private object _GetTestsLock = new object();
        #endregion

        #region Constructors
        public TestsActivitySensor(IEnumerable<IReporter> reporters, IReportContentSerializer reportContentSerializer)
        {
            if (reporters == null)
            {
                throw new ArgumentNullException("reporters");
            }

            if (reportContentSerializer == null)
            {
                throw new ArgumentNullException("reportContentSerializer");
            }

            _Reporters = reporters;
            _ReportContentSerializer = reportContentSerializer;
        }
        #endregion

        #region Override methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#")]
        public override void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                base.OnConnection(application, connectMode, addInInst, ref custom);

                Microsoft.VisualStudio.OLE.Interop.IServiceProvider InteropServiceProvider = application as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                _ServiceProvider = new ServiceProvider(InteropServiceProvider);
                _ComponentModel = (IComponentModel)_ServiceProvider.GetService(typeof(SComponentModel));
                _OperationState = _ComponentModel.GetService<IOperationState>();
                _OperationState.StateChanged += MyTestStateChanged;

                _TestsService = _ComponentModel.GetService<Microsoft.VisualStudio.TestWindow.Extensibility.ITestsService>();
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public override void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {
            base.OnDisconnection(disconnectMode, ref custom);

            try
            {
                _OperationState.StateChanged -= MyTestStateChanged;
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }
        #endregion

        #region My methods
        private void MyReportAll(Report report)
        {
            foreach (var LoopReporter in _Reporters)
            {
                LoopReporter.Report(report);
            }
        }

        private void MyTestStateChanged(object sender, OperationStateChangedEventArgs eventArgs)
        {
            try
            {
                OperationData OperationData = sender as OperationData;
                TestRunRequest Request = eventArgs.Operation as TestRunRequest;

                if (Request != null && OperationData != null)
                {
                    switch (eventArgs.State)
                    {
                        case TestOperationStates.TestExecutionStarted:
                            MyReportAll(new Report(new object(), TestOperationStates.TestExecutionStarted.ToString(), base.ProcessId, base.SolutionName, _ReportContentSerializer));
                            break;
                        case TestOperationStates.TestExecutionFinished:
                            MyOnTestExecutionFinishedAsync(Request, OperationData.LastConfig);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        private void MyOnTestExecutionFinishedAsync(TestRunRequest request, TestRunConfiguration testRunConfiguration)
        {
            lock (_GetTestsLock)
            {
                var GetTestTask = _TestsService.GetTests();
                GetTestTask.ContinueWith(Task =>
                {
                    try
                    {
                        // Filter by current request configuration                        
                        List<ITest> SelectedTests = Task.Result.ToList();

                        var SelectedTestCases = testRunConfiguration.Tests.Select(TestCase => TestCase.Id).ToList();
                        // If TestRunConfiguration.Test.Count == 0 it means user fired Run All Tests command

                        if (SelectedTestCases.Count > 0)
                        {
                            SelectedTests = SelectedTests.Where(SingleTask => SelectedTestCases.Any(TestCase => TestCase.Equals(SingleTask.Id))).ToList();
                        }
                        
                        var ReportModel = new TestExecutionFinishedReportModel(request, SelectedTests);
                        MyReportAll(new Report(ReportModel, TestOperationStates.TestExecutionFinished.ToString(), base.ProcessId, base.SolutionName, _ReportContentSerializer));
                    }
                    catch (Exception exception)
                    {
                        throw new SensorException(exception.Message, exception);
                    }
                });
            }
        }
        #endregion

        #region IDisposable methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ServiceProvider != null)
                {
                    _ServiceProvider.Dispose();
                    _ServiceProvider = null;
                }
            }
        }
        #endregion
    }
}