using EnvDTE;
using EnvDTE80;
using Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public class Distributor
    {
        #region Private variables
        private int _NumberOfSecondsToSetInactive = 60;
        private DTE2 _ApplicationObject;
        private SolutionEvents _SolutionEvents;
        private BuildEvents _BuildEvents;
        private bool _IsActive;
        System.Timers.Timer _Timer;
        private int _ProcessId;
        #endregion

        #region Constructors
        public Distributor(IEnumerable<IActivitySensor> sensors)
        {
            _ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            _Timer = new System.Timers.Timer(_NumberOfSecondsToSetInactive * 1000);
            _IsActive = true;
        }
        #endregion

        #region Solution events
        void solutionEvents_Opened()
        {
            TickAlive();
        }

        void solutionEvents_BeforeClosing()
        {
            TickAlive();
        }
        #endregion

        #region Build events
        void _buildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            TickAlive();
        }

        void _buildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            TickAlive();
        }

        void buildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            TickAlive();
        }

        void _buildEvents_OnBuildProjConfigBegin(string Project, string ProjectConfig, string Platform, string SolutionConfig)
        {
            TickAlive();
        }
        #endregion

        #region Various (tick only) events
        void WindowEvents_WindowMoved(Window Window, int Top, int Left, int Width, int Height)
        {
            TickAlive();
        }

        void WindowEvents_WindowCreated(Window Window)
        {
            TickAlive();
        }

        void WindowEvents_WindowClosing(Window Window)
        {
            TickAlive();
        }

        void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            TickAlive();
        }

        void TextEditorEvents_LineChanged(TextPoint StartPoint, TextPoint EndPoint, int Hint)
        {
            TickAlive();
        }

        void TaskListEvents_TaskRemoved(TaskItem TaskItem)
        {
            TickAlive();
        }

        void TaskListEvents_TaskNavigated(TaskItem TaskItem, ref bool NavigateHandled)
        {
            TickAlive();
        }

        void TaskListEvents_TaskModified(TaskItem TaskItem, vsTaskListColumn ColumnModified)
        {
            TickAlive();
        }

        void TaskListEvents_TaskAdded(TaskItem TaskItem)
        {
            TickAlive();
        }

        void SolutionItemsEvents_ItemRenamed(ProjectItem ProjectItem, string OldName)
        {
            TickAlive();
        }

        void SolutionItemsEvents_ItemRemoved(ProjectItem ProjectItem)
        {
            TickAlive();
        }

        void SolutionItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            TickAlive();
        }

        void SolutionEvents_Renamed(string OldName)
        {
            TickAlive();
        }

        void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            TickAlive();
        }

        void SolutionEvents_ProjectRenamed(Project Project, string OldName)
        {
            TickAlive();
        }

        void SolutionEvents_ProjectRemoved(Project Project)
        {
            TickAlive();
        }

        void SolutionEvents_ProjectAdded(Project Project)
        {
            TickAlive();
        }

        void SolutionEvents_Opened()
        {
            TickAlive();
        }

        void SolutionEvents_BeforeClosing()
        {
            TickAlive();
        }

        void SolutionEvents_AfterClosing()
        {
            TickAlive();
        }

        void SelectionEvents_OnChange()
        {
            TickAlive();
        }

        void OutputWindowEvents_PaneUpdated(OutputWindowPane pPane)
        {
            TickAlive();
        }

        void OutputWindowEvents_PaneClearing(OutputWindowPane pPane)
        {
            TickAlive();
        }

        void OutputWindowEvents_PaneAdded(OutputWindowPane pPane)
        {
            TickAlive();
        }

        void MiscFilesEvents_ItemRenamed(ProjectItem ProjectItem, string OldName)
        {
            TickAlive();
        }

        void MiscFilesEvents_ItemRemoved(ProjectItem ProjectItem)
        {
            TickAlive();
        }

        void MiscFilesEvents_ItemAdded(ProjectItem ProjectItem)
        {
            TickAlive();
        }

        void FindEvents_FindDone(vsFindResult Result, bool Cancelled)
        {
            TickAlive();
        }

        void DebuggerEvents_OnExceptionThrown(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
        {
            TickAlive();
        }

        void DebuggerEvents_OnExceptionNotHandled(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
        {
            TickAlive();
        }

        void DebuggerEvents_OnEnterRunMode(dbgEventReason Reason)
        {
            TickAlive();
        }

        void DebuggerEvents_OnEnterDesignMode(dbgEventReason Reason)
        {
            TickAlive();
        }

        void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
        {
            TickAlive();
        }

        void DebuggerEvents_OnContextChanged(EnvDTE.Process NewProcess, Program NewProgram, Thread NewThread, EnvDTE.StackFrame NewStackFrame)
        {
            TickAlive();
        }

        void CommandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            TickAlive();
        }

        void CommandEvents_AfterExecute(string Guid, int ID, object CustomIn, object CustomOut)
        {
            TickAlive();
        }

        void DocumentEventsTick(Document Document)
        {
            TickAlive();
        }

        #endregion

        #region Public
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {

            _Timer.Elapsed += _timer_Elapsed;
            _Timer.Start();

            _ApplicationObject = (DTE2)application;

            _ApplicationObject.Events.DocumentEvents.DocumentClosing += DocumentEventsTick;
            _ApplicationObject.Events.DocumentEvents.DocumentSaved += DocumentEventsTick;
            _ApplicationObject.Events.DocumentEvents.DocumentOpened += DocumentEventsTick;

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
            // Add solution item events
            _SolutionEvents = _ApplicationObject.Events.SolutionEvents;
            _SolutionEvents.Opened += solutionEvents_Opened;
            _SolutionEvents.BeforeClosing += solutionEvents_BeforeClosing;

            // Add build events
            _BuildEvents = _ApplicationObject.Events.BuildEvents;
            _BuildEvents.OnBuildBegin += buildEvents_OnBuildBegin;
            _BuildEvents.OnBuildProjConfigDone += _buildEvents_OnBuildProjConfigDone;
            _BuildEvents.OnBuildDone += _buildEvents_OnBuildDone;
            _BuildEvents.OnBuildProjConfigBegin += _buildEvents_OnBuildProjConfigBegin;

            string s = _ApplicationObject.FullName;



        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_IsActive)
            {
                // Send user inactive
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
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }
        #endregion
    }
}
