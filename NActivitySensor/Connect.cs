using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using System.Diagnostics;

namespace NActivitySensor
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{
        #region Private variables
        private Logger _logger;
        private int _numberOfSecondsToSetInactive = 60;
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private SolutionEvents _solutionEvents;
        private BuildEvents _buildEvents;
        private ProjectsEvents _projectsEvents;
        private DocumentEvents _documentEvents;
        private List<string> _projectsBuildReport;
        private bool IsAlive;
        System.Timers.Timer _timer;
        private int _ProcessId;
        #endregion

        #region Solution events
        void solutionEvents_Opened()
        {
            _logger.Log("solutionEvents_Opened(): " + _applicationObject.Solution.FullName);
            TickAlive();
        }

        void solutionEvents_BeforeClosing()
        {
            _logger.Log("solutionEvents_BeforeClosing(): " + _applicationObject.Solution.FullName);
            TickAlive();
        }
        #endregion

        #region Build events
        void _buildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            _projectsBuildReport.Add(DateTime.Now.ToUniversalTime().ToString() + "  " + (Success ? "Succeeded" : "Failed   ") + " | " + Project + " [" + ProjectConfig + "|" + Platform + "]");
            _logger.Log("buildEvents_OnBuildProjConfigDone: " + DateTime.Now.ToUniversalTime().ToString() + "  " + (Success ? "Succeeded" : "Failed   ") + " | " + Project + " [" + ProjectConfig + "|" + Platform + "]");
            TickAlive();
        }

        void _buildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            _projectsBuildReport.Add("Build scope:" + Scope.ToString());
            _projectsBuildReport.Add("Build action:" + Action.ToString());
            TickAlive();
        }

        void buildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            _projectsBuildReport.Clear();
            _logger.Log("buildEvents_OnBuildBegin(): " + DateTime.Now.ToUniversalTime().ToString() + " (Scope: " + Scope.ToString() + ", Action: " + Action.ToString() + ")");
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
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
            _ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            _logger = new Logger(_ProcessId);
            
            _timer = new System.Timers.Timer(_numberOfSecondsToSetInactive * 1000);
            _projectsBuildReport = new List<string>();
            IsAlive = true;
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            _applicationObject.Events.DocumentEvents.DocumentClosing += DocumentEventsTick;
            _applicationObject.Events.DocumentEvents.DocumentSaved += DocumentEventsTick;
            _applicationObject.Events.DocumentEvents.DocumentOpened += DocumentEventsTick;

            _applicationObject.Events.CommandEvents.AfterExecute += CommandEvents_AfterExecute;
            _applicationObject.Events.CommandEvents.BeforeExecute += CommandEvents_BeforeExecute;

            _applicationObject.Events.DebuggerEvents.OnContextChanged += DebuggerEvents_OnContextChanged;
            _applicationObject.Events.DebuggerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
            _applicationObject.Events.DebuggerEvents.OnEnterDesignMode += DebuggerEvents_OnEnterDesignMode;
            _applicationObject.Events.DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
            _applicationObject.Events.DebuggerEvents.OnExceptionNotHandled += DebuggerEvents_OnExceptionNotHandled;
            _applicationObject.Events.DebuggerEvents.OnExceptionThrown += DebuggerEvents_OnExceptionThrown;

            _applicationObject.Events.FindEvents.FindDone += FindEvents_FindDone;

            _applicationObject.Events.MiscFilesEvents.ItemAdded += MiscFilesEvents_ItemAdded;
            _applicationObject.Events.MiscFilesEvents.ItemRemoved += MiscFilesEvents_ItemRemoved;
            _applicationObject.Events.MiscFilesEvents.ItemRenamed += MiscFilesEvents_ItemRenamed;

            _applicationObject.Events.OutputWindowEvents.PaneAdded += OutputWindowEvents_PaneAdded;
            _applicationObject.Events.OutputWindowEvents.PaneClearing += OutputWindowEvents_PaneClearing;
            _applicationObject.Events.OutputWindowEvents.PaneUpdated += OutputWindowEvents_PaneUpdated;

            _applicationObject.Events.SelectionEvents.OnChange += SelectionEvents_OnChange;
            _applicationObject.Events.SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
            _applicationObject.Events.SolutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
            _applicationObject.Events.SolutionEvents.Opened += SolutionEvents_Opened;
            _applicationObject.Events.SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
            _applicationObject.Events.SolutionEvents.ProjectRemoved += SolutionEvents_ProjectRemoved;
            _applicationObject.Events.SolutionEvents.ProjectRenamed += SolutionEvents_ProjectRenamed;
            _applicationObject.Events.SolutionEvents.QueryCloseSolution += SolutionEvents_QueryCloseSolution;
            _applicationObject.Events.SolutionEvents.Renamed += SolutionEvents_Renamed;

            _applicationObject.Events.SolutionItemsEvents.ItemAdded += SolutionItemsEvents_ItemAdded;
            _applicationObject.Events.SolutionItemsEvents.ItemRemoved += SolutionItemsEvents_ItemRemoved;
            _applicationObject.Events.SolutionItemsEvents.ItemRenamed += SolutionItemsEvents_ItemRenamed;

            _applicationObject.Events.TaskListEvents.TaskAdded += TaskListEvents_TaskAdded;
            _applicationObject.Events.TaskListEvents.TaskModified += TaskListEvents_TaskModified;
            _applicationObject.Events.TaskListEvents.TaskNavigated += TaskListEvents_TaskNavigated;
            _applicationObject.Events.TaskListEvents.TaskRemoved += TaskListEvents_TaskRemoved;

            _applicationObject.Events.TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;

            _applicationObject.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;
            _applicationObject.Events.WindowEvents.WindowClosing += WindowEvents_WindowClosing;
            _applicationObject.Events.WindowEvents.WindowCreated += WindowEvents_WindowCreated;
            _applicationObject.Events.WindowEvents.WindowMoved += WindowEvents_WindowMoved;
            // Add solution item events
            _solutionEvents = _applicationObject.Events.SolutionEvents;
            _solutionEvents.Opened += solutionEvents_Opened;
            _solutionEvents.BeforeClosing += solutionEvents_BeforeClosing;

            // Add build events
            _buildEvents = _applicationObject.Events.BuildEvents;
            _buildEvents.OnBuildBegin += buildEvents_OnBuildBegin;
            _buildEvents.OnBuildProjConfigDone += _buildEvents_OnBuildProjConfigDone;
            _buildEvents.OnBuildDone += _buildEvents_OnBuildDone;
            _buildEvents.OnBuildProjConfigBegin += _buildEvents_OnBuildProjConfigBegin;

            string s = _applicationObject.FullName;
            
            

        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsAlive)
            {
                _projectsBuildReport.Add(DateTime.Now.ToUniversalTime().ToString() + " User not active!");
                _logger.Log("User not active.");
            }

            IsAlive = false;
        }


        private void TickAlive()
        {
            IsAlive = true;
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