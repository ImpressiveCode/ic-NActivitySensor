using EnvDTE;
using EnvDTE80;
using Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    /// <summary>
    /// Distributes events to the sensors
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Distributor : IDisposable
    {
        #region Private variables
        private int _NumberOfSecondsToSetInactive = 60;
        private DTE2 _ApplicationObject;
        private BuildEvents _BuildEvents;
        private bool _IsActive;
        System.Timers.Timer _Timer;
        private int _ProcessId;
        private readonly IEnumerable<IActivitySensor> _Sensors;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Distributor"/> class.
        /// </summary>
        /// <param name="sensors">The sensors.</param>
        /// <exception cref="System.ArgumentNullException">sensors</exception>
        public Distributor(IEnumerable<IActivitySensor> sensors)
        {
            if (sensors == null)
            {
                throw new ArgumentNullException("sensors");
            }

            _Sensors = sensors;
            _ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            _Timer = new System.Timers.Timer(_NumberOfSecondsToSetInactive * 1000);
            _IsActive = true;
        }
        #endregion

        #region Solution events
        void SolutionEvents_Opened()
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionOpened(String.Empty);
            }

            TickAlive();
        }

        void SolutionEvents_BeforeClosing()
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionBeforeClosing(String.Empty);
            }

            TickAlive();
        }
        #endregion

        #region Build events
        void BuildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildProjConfigDone(project, projectConfig, platform, solutionConfig, success);
            }

            TickAlive();
        }

        void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildDone(scope, action);
            }

            TickAlive();
        }

        void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildBegin(scope, action);
            }

            TickAlive();
        }

        void BuildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildProjConfigBegin(project, projectConfig, platform, solutionConfig);
            }

            TickAlive();
        }
        #endregion

        #region Various (tick only) events
        void WindowEvents_WindowMoved(Window window, int top, int left, int width, int height)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowMoved(window, top, left, width, height);
            }

            TickAlive();
        }

        void WindowEvents_WindowCreated(Window window)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowCreated(window);
            }

            TickAlive();
        }

        void WindowEvents_WindowClosing(Window window)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowClosing(window);
            }

            TickAlive();
        }

        void WindowEvents_WindowActivated(Window gotFocus, Window lostFocus)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowActivated(gotFocus, lostFocus);
            }

            TickAlive();
        }

        void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnLineChanged(startPoint, endPoint, hint);
            }

            TickAlive();
        }

        void TaskListEvents_TaskRemoved(TaskItem taskItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskRemoved(taskItem);
            }

            TickAlive();
        }

        void TaskListEvents_TaskNavigated(TaskItem taskItem, ref bool navigateHandled)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskNavigated(taskItem, ref navigateHandled);
            }

            TickAlive();
        }

        void TaskListEvents_TaskModified(TaskItem taskItem, vsTaskListColumn columnModified)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskModified(taskItem, columnModified);
            }

            TickAlive();
        }

        void TaskListEvents_TaskAdded(TaskItem taskItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskAdded(taskItem);
            }

            TickAlive();
        }

        void SolutionItemsEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRenamed(projectItem, oldName);
            }

            TickAlive();
        }

        void SolutionItemsEvents_ItemRemoved(ProjectItem projectItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRemoved(projectItem);
            }

            TickAlive();
        }

        void SolutionItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            TickAlive();
        }

        void SolutionEvents_Renamed(string oldName)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionRenamed(oldName);
            }

            TickAlive();
        }

        void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionQueryClose(ref fCancel);
            }

            TickAlive();
        }

        void SolutionEvents_ProjectRenamed(Project project, string oldName)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectRenamed(project, oldName);
            }

            TickAlive();
        }

        void SolutionEvents_ProjectRemoved(Project project)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectRemoved(project);
            }

            TickAlive();
        }

        void SolutionEvents_ProjectAdded(Project project)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectAdded(project);
            }

            TickAlive();
        }

        void SolutionEvents_AfterClosing()
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionAfterClosing(String.Empty);
            }

            TickAlive();
        }

        void SelectionEvents_OnChange()
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSelectionChange();
            }

            TickAlive();
        }

        void OutputWindowEvents_PaneUpdated(OutputWindowPane pPane)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneAdded(pPane);
            }

            TickAlive();
        }

        void OutputWindowEvents_PaneClearing(OutputWindowPane pPane)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneClearing(pPane);
            }

            TickAlive();
        }

        void OutputWindowEvents_PaneAdded(OutputWindowPane pPane)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneAdded(pPane);
            }

            TickAlive();
        }

        void MiscFilesEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRenamed(projectItem, oldName);
            }

            TickAlive();
        }

        void MiscFilesEvents_ItemRemoved(ProjectItem projectItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRemoved(projectItem);
            }

            TickAlive();
        }

        void MiscFilesEvents_ItemAdded(ProjectItem projectItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemAdded(projectItem);
            }

            TickAlive();
        }

        void FindEvents_FindDone(vsFindResult result, bool cancelled)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFindDone(result, cancelled);
            }

            TickAlive();
        }

        void DebuggerEvents_OnExceptionThrown(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerExceptionThrown(exceptionType, name, code, description, ref exceptionAction);
            }

            TickAlive();
        }

        void DebuggerEvents_OnExceptionNotHandled(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerExceptionNotHandled(exceptionType, name, code, description, ref exceptionAction);
            }

            TickAlive();
        }

        void DebuggerEvents_OnEnterRunMode(dbgEventReason reason)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterRunMode(reason);
            }

            TickAlive();
        }

        void DebuggerEvents_OnEnterDesignMode(dbgEventReason reason)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterDesignMode(reason);
            }

            TickAlive();
        }

        void DebuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterBreakMode(reason, ref executionAction);
            }

            TickAlive();
        }

        void DebuggerEvents_OnContextChanged(EnvDTE.Process newProcess, Program newProgram, Thread newThread, EnvDTE.StackFrame newStackFrame)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerContextChanged(newProcess, newProgram, newThread, newStackFrame);
            }

            TickAlive();
        }

        void CommandEvents_BeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnCommandBeforeExecute(guid, id, customIn, customOut, ref cancelDefault);
            }

            TickAlive();
        }

        void CommandEvents_AfterExecute(string guid, int id, object customIn, object customOut)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnCommandAfterExecute(guid, id, customIn, customOut);
            }

            TickAlive();
        }

        void OnDocumentClosing(Document document)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentClosing(document);
            }

            TickAlive();
        }

        void OnDocumentSaved(Document document)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentSaved(document);
            }

            TickAlive();
        }

        void OnDocumentOpened(Document document)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentOpened(document);
            }

            TickAlive();
        }

        #endregion

        #region Public
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "addInInst"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "custom"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "connectMode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst")]
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _Timer.Elapsed += OnTimerElapsed;
            _Timer.Start();

            _ApplicationObject = (DTE2)application;

            _ApplicationObject.Events.DocumentEvents.DocumentClosing += OnDocumentClosing;
            _ApplicationObject.Events.DocumentEvents.DocumentSaved += OnDocumentSaved;
            _ApplicationObject.Events.DocumentEvents.DocumentOpened += OnDocumentOpened;

            _ApplicationObject.Events.CommandEvents.AfterExecute += CommandEvents_AfterExecute;
            _ApplicationObject.Events.CommandEvents.BeforeExecute += CommandEvents_BeforeExecute;

            _ApplicationObject.Events.DebuggerEvents.OnContextChanged += DebuggerEvents_OnContextChanged;
            _ApplicationObject.Events.DebuggerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
            _ApplicationObject.Events.DebuggerEvents.OnEnterDesignMode += DebuggerEvents_OnEnterDesignMode;
            _ApplicationObject.Events.DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
            _ApplicationObject.Events.DebuggerEvents.OnExceptionNotHandled += DebuggerEvents_OnExceptionNotHandled;
            _ApplicationObject.Events.DebuggerEvents.OnExceptionThrown += DebuggerEvents_OnExceptionThrown;

            _ApplicationObject.Events.FindEvents.FindDone += FindEvents_FindDone;

            _ApplicationObject.Events.MiscFilesEvents.ItemAdded += MiscFilesEvents_ItemAdded;
            _ApplicationObject.Events.MiscFilesEvents.ItemRemoved += MiscFilesEvents_ItemRemoved;
            _ApplicationObject.Events.MiscFilesEvents.ItemRenamed += MiscFilesEvents_ItemRenamed;

            _ApplicationObject.Events.OutputWindowEvents.PaneAdded += OutputWindowEvents_PaneAdded;
            _ApplicationObject.Events.OutputWindowEvents.PaneClearing += OutputWindowEvents_PaneClearing;
            _ApplicationObject.Events.OutputWindowEvents.PaneUpdated += OutputWindowEvents_PaneUpdated;

            _ApplicationObject.Events.SelectionEvents.OnChange += SelectionEvents_OnChange;

            _ApplicationObject.Events.SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
            _ApplicationObject.Events.SolutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
            _ApplicationObject.Events.SolutionEvents.Opened += SolutionEvents_Opened;
            _ApplicationObject.Events.SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
            _ApplicationObject.Events.SolutionEvents.ProjectRemoved += SolutionEvents_ProjectRemoved;
            _ApplicationObject.Events.SolutionEvents.ProjectRenamed += SolutionEvents_ProjectRenamed;
            _ApplicationObject.Events.SolutionEvents.QueryCloseSolution += SolutionEvents_QueryCloseSolution;
            _ApplicationObject.Events.SolutionEvents.Renamed += SolutionEvents_Renamed;

            _ApplicationObject.Events.SolutionItemsEvents.ItemAdded += SolutionItemsEvents_ItemAdded;
            _ApplicationObject.Events.SolutionItemsEvents.ItemRemoved += SolutionItemsEvents_ItemRemoved;
            _ApplicationObject.Events.SolutionItemsEvents.ItemRenamed += SolutionItemsEvents_ItemRenamed;

            _ApplicationObject.Events.TaskListEvents.TaskAdded += TaskListEvents_TaskAdded;
            _ApplicationObject.Events.TaskListEvents.TaskModified += TaskListEvents_TaskModified;
            _ApplicationObject.Events.TaskListEvents.TaskNavigated += TaskListEvents_TaskNavigated;
            _ApplicationObject.Events.TaskListEvents.TaskRemoved += TaskListEvents_TaskRemoved;

            _ApplicationObject.Events.TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;

            _ApplicationObject.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;
            _ApplicationObject.Events.WindowEvents.WindowClosing += WindowEvents_WindowClosing;
            _ApplicationObject.Events.WindowEvents.WindowCreated += WindowEvents_WindowCreated;
            _ApplicationObject.Events.WindowEvents.WindowMoved += WindowEvents_WindowMoved;

            // Add build events
            _BuildEvents = _ApplicationObject.Events.BuildEvents;
            _BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            _BuildEvents.OnBuildProjConfigDone += BuildEvents_OnBuildProjConfigDone;
            _BuildEvents.OnBuildDone += BuildEvents_OnBuildDone;
            _BuildEvents.OnBuildProjConfigBegin += BuildEvents_OnBuildProjConfigBegin;

            string s = _ApplicationObject.FullName;



        }

        void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_IsActive)
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnUserInactive();
                }
            }

            _IsActive = false;
        }

        private void TickAlive()
        {
            _IsActive = true;
        }


        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDisconnection(disconnectMode, ref custom);
            }

            TickAlive();
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnAddInsUpdate(ref Array custom)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnAddInsUpdate(ref custom);
            }

            TickAlive();
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnStartupComplete(ref Array custom)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnStartupComplete(ref custom);
            }

            TickAlive();
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnBeginShutdown(ref Array custom)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBeginShutdown(ref custom);
            }

            TickAlive();
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
                if (_Timer != null)
                {
                    _Timer.Dispose();
                    _Timer = null;
                }
            }
        }
        #endregion
    }
}
