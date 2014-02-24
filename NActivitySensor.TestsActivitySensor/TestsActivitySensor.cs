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

namespace NActivitySensor
{   
    /// <summary>
    /// C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow
    /// </summary>
    public class TestsActivitySensor : BaseActivitySensor
    {
        private IServiceProvider _ServiceProvider;
        private IComponentModel _ComponentModel;
        private IOperationState _OperationState;

        private readonly IEnumerable<IReporter> _Reporters;

        public TestsActivitySensor(IEnumerable<IReporter> reporters)
        {
            if (reporters == null)
            {
                throw new ArgumentNullException("reporters");
            }

            _Reporters = reporters;
        }

        private void MyReportAll(Report report)
        {
            foreach (var LoopReporter in _Reporters)
            {
                LoopReporter.Report(report);
            }
        }   

        public override void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {            
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider InteropServiceProvider = application as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
            _ServiceProvider = new ServiceProvider(InteropServiceProvider);
            _ComponentModel = (IComponentModel)_ServiceProvider.GetService(typeof(SComponentModel));
            _OperationState = _ComponentModel.GetService<IOperationState>();
            _OperationState.StateChanged += _OperationState_StateChanged;
        }

        void _OperationState_StateChanged(object sender, OperationStateChangedEventArgs e)
        {
           
        }
    }
}
