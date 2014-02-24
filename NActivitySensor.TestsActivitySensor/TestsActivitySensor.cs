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
    using Microsoft.VisualStudio.TestTools.Execution;
    #endregion

    /// <summary>
    /// C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow
    /// </summary>
    public class TestsActivitySensor : BaseActivitySensor
    {
        #region Private variables
        private IServiceProvider _ServiceProvider;
        private IComponentModel _ComponentModel;
        private IOperationState _OperationState;

        private readonly IEnumerable<IReporter> _Reporters;
        private readonly IReportContentSerializer _ReportContentSerializer;
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

        private void MyReportAll(Report report)
        {
            foreach (var LoopReporter in _Reporters)
            {
                LoopReporter.Report(report);
            }
        }

        #region Override methods
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
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }
        #endregion

        #region My methods
        private void MyTestStateChanged(object sender, OperationStateChangedEventArgs e)
        {
            try
            {
                MyReportAll(new Report(e.State, e.State.ToString(), base.ProcessId, base.SolutionFullName, _ReportContentSerializer));
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }
        #endregion
    }
}
