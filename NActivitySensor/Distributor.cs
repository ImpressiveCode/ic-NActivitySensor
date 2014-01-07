namespace NActivitySensor
{
    #region Usings
    using EnvDTE;
    using EnvDTE80;
    using Extensibility;
    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Distributes events to the sensors
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Distributor : IDisposable
    {
        #region Private variables
        private int _NumberOfSecondsToSetInactive = 10;

        private DTE2 _ApplicationObject;
        private Events _Events;
        private DocumentEvents _DocumentEvents;
        private CommandEvents _CommandEvents;
        private DebuggerEvents _DebuggerEvents;
        private FindEvents _FindEvents;
        private ProjectItemsEvents _MiscFilesEvents;
        private OutputWindowEvents _OutputWindowEvents;
        private SelectionEvents _SelectionEvents;
        private SolutionEvents _SolutionEvents;
        private ProjectItemsEvents _SolutionItemsEvents;
        private TaskListEvents _TaskListEvents;
        private TextEditorEvents _TextEditorEvents;
        private WindowEvents _WindowEvents;
        private BuildEvents _BuildEvents;

        private bool _IsActive;
        private bool _IsActiveNotified;
        private bool _IsInactiveNotified;
        System.Timers.Timer _Timer;
        private static object _TimerLock = new object();

        private int _ProcessId;
        private IEnumerable<IActivitySensor> _Sensors;
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
            _Timer = new System.Timers.Timer(_NumberOfSecondsToSetInactive * 1000);
            _IsActive = true;

            _ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnConnect(_ProcessId);
            }
        }
        #endregion

        #region Solution events
        void SolutionEvents_Opened()
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionOpened();
            }
        }

        void SolutionEvents_BeforeClosing()
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionBeforeClosing();
            }
        }
        #endregion

        #region Build events
        void BuildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildProjConfigDone(project, projectConfig, platform, solutionConfig, success);
            }

            MyTickAlive();
        }

        void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildDone(scope, action);
            }

            MyTickAlive();
        }

        void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildBegin(scope, action);
            }

            MyTickAlive();
        }

        void BuildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBuildProjConfigBegin(project, projectConfig, platform, solutionConfig);
            }

            MyTickAlive();
        }
        #endregion

        #region Window events
        void WindowEvents_WindowMoved(Window window, int top, int left, int width, int height)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowMoved(window, top, left, width, height);
            }
        }

        void WindowEvents_WindowCreated(Window window)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowCreated(window);
            }
        }

        void WindowEvents_WindowClosing(Window window)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowClosing(window);
            }
        }

        void WindowEvents_WindowActivated(Window gotFocus, Window lostFocus)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowActivated(gotFocus, lostFocus);
            }
        }
        #endregion

        #region TextEditor events
        void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnLineChanged(startPoint, endPoint, hint);
            }
        }
        #endregion

        #region TaskList events
        void TaskListEvents_TaskRemoved(TaskItem taskItem)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskRemoved(taskItem);
            }
        }

        void TaskListEvents_TaskNavigated(TaskItem taskItem, ref bool navigateHandled)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskNavigated(taskItem, ref navigateHandled);
            }

            MyTickAlive();
        }

        void TaskListEvents_TaskModified(TaskItem taskItem, vsTaskListColumn columnModified)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskModified(taskItem, columnModified);
            }

            MyTickAlive();
        }

        void TaskListEvents_TaskAdded(TaskItem taskItem)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnTaskAdded(taskItem);
            }

            MyTickAlive();
        }
        #endregion

        #region Solution items events
        void SolutionItemsEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRenamed(projectItem, oldName);
            }
        }

        void SolutionItemsEvents_ItemRemoved(ProjectItem projectItem)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRemoved(projectItem);
            }
        }

        void SolutionItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemAdded(ProjectItem);
            }
        }
        #endregion

        #region Solution events
        void SolutionEvents_Renamed(string oldName)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionRenamed(oldName);
            }
        }

        void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionQueryClose(ref fCancel);
            }
        }

        void SolutionEvents_ProjectRenamed(Project project, string oldName)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectRenamed(project, oldName);
            }
        }

        void SolutionEvents_ProjectRemoved(Project project)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectRemoved(project);
            }
        }

        void SolutionEvents_ProjectAdded(Project project)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionProjectAdded(project);
            }
        }

        void SolutionEvents_AfterClosing()
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSolutionAfterClosing();
            }
        }

        void SelectionEvents_OnChange()
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnSelectionChange();
            }
        }
        #endregion

        #region Output window events
        void OutputWindowEvents_PaneUpdated(OutputWindowPane pPane)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneAdded(pPane);
            }
        }

        void OutputWindowEvents_PaneClearing(OutputWindowPane pPane)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneClearing(pPane);
            }
        }

        void OutputWindowEvents_PaneAdded(OutputWindowPane pPane)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnWindowPaneAdded(pPane);
            }
        }
        #endregion

        #region Files events
        void MiscFilesEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRenamed(projectItem, oldName);
            }
        }

        void MiscFilesEvents_ItemRemoved(ProjectItem projectItem)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemRemoved(projectItem);
            }
        }

        void MiscFilesEvents_ItemAdded(ProjectItem projectItem)
        {

            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFileItemAdded(projectItem);
            }
        }
        #endregion

        #region Find events
        void FindEvents_FindDone(vsFindResult result, bool cancelled)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnFindDone(result, cancelled);
            }
        }
        #endregion

        #region Debugger events
        void DebuggerEvents_OnExceptionThrown(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerExceptionThrown(exceptionType, name, code, description, ref exceptionAction);
            }
        }

        void DebuggerEvents_OnExceptionNotHandled(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerExceptionNotHandled(exceptionType, name, code, description, ref exceptionAction);
            }
        }

        void DebuggerEvents_OnEnterRunMode(dbgEventReason reason)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterRunMode(reason);
            }
        }

        void DebuggerEvents_OnEnterDesignMode(dbgEventReason reason)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterDesignMode(reason);
            }
        }

        void DebuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerEnterBreakMode(reason, ref executionAction);
            }
        }

        void DebuggerEvents_OnContextChanged(EnvDTE.Process newProcess, Program newProgram, Thread newThread, EnvDTE.StackFrame newStackFrame)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDebuggerContextChanged(newProcess, newProgram, newThread, newStackFrame);
            }
        }
        #endregion

        #region Command events
        void CommandEvents_BeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnCommandBeforeExecute(guid, id, customIn, customOut, ref cancelDefault);
            }
        }

        void CommandEvents_AfterExecute(string guid, int id, object customIn, object customOut)
        {
            foreach (var Sensor in _Sensors)
            {
                Sensor.OnCommandAfterExecute(guid, id, customIn, customOut);
            }

            MyTickAlive();
        }
        #endregion

        #region Document events
        void OnDocumentClosing(Document document)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentClosing(document);
            }
        }

        void OnDocumentSaved(Document document)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentSaved(document);
            }
        }

        void OnDocumentOpened(Document document)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDocumentOpened(document);
            }
        }
        #endregion

        #region Methods
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "addInInst"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "custom"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "connectMode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst")]
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _Timer.Elapsed += MyOnTimerElapsed;
            _Timer.Start();

            _ApplicationObject = (DTE2)application;
            // GC.KeepAlive(_ApplicationObject);

            _Events = _ApplicationObject.Events;
            // GC.KeepAlive(_Events);

            _DocumentEvents = _Events.DocumentEvents;
            // GC.KeepAlive(_DocumentEvents);

            _CommandEvents = _Events.CommandEvents;
            // GC.KeepAlive(_CommandEvents);

            _DebuggerEvents = _Events.DebuggerEvents;
            // GC.KeepAlive(_DebuggerEvents);

            _DocumentEvents = _Events.DocumentEvents;
            // GC.KeepAlive(_DocumentEvents);

            _FindEvents = _Events.FindEvents;
            // GC.KeepAlive(_FindEvents);

            _MiscFilesEvents = _Events.MiscFilesEvents;
            // GC.KeepAlive(_MiscFilesEvents);

            _OutputWindowEvents = _Events.OutputWindowEvents;
            // GC.KeepAlive(this._OutputWindowEvents);

            _SelectionEvents = _Events.SelectionEvents;
            // GC.KeepAlive(_SelectionEvents);

            _SolutionEvents = _Events.SolutionEvents;
            // GC.KeepAlive(_SolutionEvents);

            _SolutionItemsEvents = _Events.SolutionItemsEvents;
            // GC.KeepAlive(_SolutionItemsEvents);

            _TaskListEvents = _Events.TaskListEvents;
            // GC.KeepAlive(_TaskListEvents);

            _TextEditorEvents = _Events.TextEditorEvents;
            // GC.KeepAlive(_TextEditorEvents);

            _WindowEvents = _Events.WindowEvents;
            // GC.KeepAlive(_WindowEvents);

            _BuildEvents = _Events.BuildEvents;

            // Documents events
            _DocumentEvents.DocumentClosing += OnDocumentClosing;
            _DocumentEvents.DocumentSaved += OnDocumentSaved;
            _DocumentEvents.DocumentOpened += OnDocumentOpened;
            
            // Command events
            _CommandEvents.AfterExecute += CommandEvents_AfterExecute;
            _CommandEvents.BeforeExecute += CommandEvents_BeforeExecute;

            // Debugger events
            _DebuggerEvents.OnContextChanged += DebuggerEvents_OnContextChanged;
            _DebuggerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
            _DebuggerEvents.OnEnterDesignMode += DebuggerEvents_OnEnterDesignMode;
            _DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
            _DebuggerEvents.OnExceptionNotHandled += DebuggerEvents_OnExceptionNotHandled;
            _DebuggerEvents.OnExceptionThrown += DebuggerEvents_OnExceptionThrown;

            // Find events
            _FindEvents.FindDone += FindEvents_FindDone;

            // Misc files events
            _MiscFilesEvents.ItemAdded += MiscFilesEvents_ItemAdded;
            _MiscFilesEvents.ItemRemoved += MiscFilesEvents_ItemRemoved;
            _MiscFilesEvents.ItemRenamed += MiscFilesEvents_ItemRenamed;

            // Output window events
            _OutputWindowEvents.PaneAdded += OutputWindowEvents_PaneAdded;
            _OutputWindowEvents.PaneClearing += OutputWindowEvents_PaneClearing;
            _OutputWindowEvents.PaneUpdated += OutputWindowEvents_PaneUpdated;

            // Selection events
            _SelectionEvents.OnChange += SelectionEvents_OnChange;

            // Solution events
            _SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
            _SolutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
            _SolutionEvents.Opened += SolutionEvents_Opened;
            _SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
            _SolutionEvents.ProjectRemoved += SolutionEvents_ProjectRemoved;
            _SolutionEvents.ProjectRenamed += SolutionEvents_ProjectRenamed;
            _SolutionEvents.QueryCloseSolution += SolutionEvents_QueryCloseSolution;
            _SolutionEvents.Renamed += SolutionEvents_Renamed;

            // Solution items events
            _SolutionItemsEvents.ItemAdded += SolutionItemsEvents_ItemAdded;
            _SolutionItemsEvents.ItemRemoved += SolutionItemsEvents_ItemRemoved;
            _SolutionItemsEvents.ItemRenamed += SolutionItemsEvents_ItemRenamed;

            // Task list events
            _TaskListEvents.TaskAdded += TaskListEvents_TaskAdded;
            _TaskListEvents.TaskModified += TaskListEvents_TaskModified;
            _TaskListEvents.TaskNavigated += TaskListEvents_TaskNavigated;
            _TaskListEvents.TaskRemoved += TaskListEvents_TaskRemoved;

            // Text editor events
            _TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;

            // Window activated events
            _WindowEvents.WindowActivated += WindowEvents_WindowActivated;
            _WindowEvents.WindowClosing += WindowEvents_WindowClosing;
            _WindowEvents.WindowCreated += WindowEvents_WindowCreated;
            _WindowEvents.WindowMoved += WindowEvents_WindowMoved;

            // Build events            
            _BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            _BuildEvents.OnBuildProjConfigDone += BuildEvents_OnBuildProjConfigDone;
            _BuildEvents.OnBuildDone += BuildEvents_OnBuildDone;
            _BuildEvents.OnBuildProjConfigBegin += BuildEvents_OnBuildProjConfigBegin;

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnConnection(application, connectMode, addInInst, ref custom);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            MyTickAlive();

            // Documents events
            _ApplicationObject.Events.DocumentEvents.DocumentClosing -= OnDocumentClosing;
            _ApplicationObject.Events.DocumentEvents.DocumentSaved -= OnDocumentSaved;
            _ApplicationObject.Events.DocumentEvents.DocumentOpened -= OnDocumentOpened;

            // Command events
            _ApplicationObject.Events.CommandEvents.AfterExecute -= CommandEvents_AfterExecute;
            _ApplicationObject.Events.CommandEvents.BeforeExecute -= CommandEvents_BeforeExecute;

            // Debugger events
            _ApplicationObject.Events.DebuggerEvents.OnContextChanged -= DebuggerEvents_OnContextChanged;
            _ApplicationObject.Events.DebuggerEvents.OnEnterBreakMode -= DebuggerEvents_OnEnterBreakMode;
            _ApplicationObject.Events.DebuggerEvents.OnEnterDesignMode -= DebuggerEvents_OnEnterDesignMode;
            _ApplicationObject.Events.DebuggerEvents.OnEnterRunMode -= DebuggerEvents_OnEnterRunMode;
            _ApplicationObject.Events.DebuggerEvents.OnExceptionNotHandled -= DebuggerEvents_OnExceptionNotHandled;
            _ApplicationObject.Events.DebuggerEvents.OnExceptionThrown -= DebuggerEvents_OnExceptionThrown;

            // Find events
            _ApplicationObject.Events.FindEvents.FindDone -= FindEvents_FindDone;

            // Misc files events
            _ApplicationObject.Events.MiscFilesEvents.ItemAdded -= MiscFilesEvents_ItemAdded;
            _ApplicationObject.Events.MiscFilesEvents.ItemRemoved -= MiscFilesEvents_ItemRemoved;
            _ApplicationObject.Events.MiscFilesEvents.ItemRenamed -= MiscFilesEvents_ItemRenamed;

            // Output window events
            _ApplicationObject.Events.OutputWindowEvents.PaneAdded -= OutputWindowEvents_PaneAdded;
            _ApplicationObject.Events.OutputWindowEvents.PaneClearing -= OutputWindowEvents_PaneClearing;
            _ApplicationObject.Events.OutputWindowEvents.PaneUpdated -= OutputWindowEvents_PaneUpdated;

            // Selection events
            _ApplicationObject.Events.SelectionEvents.OnChange -= SelectionEvents_OnChange;

            // Solution events
            _ApplicationObject.Events.SolutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
            _ApplicationObject.Events.SolutionEvents.BeforeClosing -= SolutionEvents_BeforeClosing;
            _ApplicationObject.Events.SolutionEvents.Opened -= SolutionEvents_Opened;
            _ApplicationObject.Events.SolutionEvents.ProjectAdded -= SolutionEvents_ProjectAdded;
            _ApplicationObject.Events.SolutionEvents.ProjectRemoved -= SolutionEvents_ProjectRemoved;
            _ApplicationObject.Events.SolutionEvents.ProjectRenamed -= SolutionEvents_ProjectRenamed;
            _ApplicationObject.Events.SolutionEvents.QueryCloseSolution -= SolutionEvents_QueryCloseSolution;
            _ApplicationObject.Events.SolutionEvents.Renamed -= SolutionEvents_Renamed;

            // Solution items events
            _ApplicationObject.Events.SolutionItemsEvents.ItemAdded -= SolutionItemsEvents_ItemAdded;
            _ApplicationObject.Events.SolutionItemsEvents.ItemRemoved -= SolutionItemsEvents_ItemRemoved;
            _ApplicationObject.Events.SolutionItemsEvents.ItemRenamed -= SolutionItemsEvents_ItemRenamed;

            // Task list events
            _ApplicationObject.Events.TaskListEvents.TaskAdded -= TaskListEvents_TaskAdded;
            _ApplicationObject.Events.TaskListEvents.TaskModified -= TaskListEvents_TaskModified;
            _ApplicationObject.Events.TaskListEvents.TaskNavigated -= TaskListEvents_TaskNavigated;
            _ApplicationObject.Events.TaskListEvents.TaskRemoved -= TaskListEvents_TaskRemoved;

            // Text editor events
            _ApplicationObject.Events.TextEditorEvents.LineChanged -= TextEditorEvents_LineChanged;

            // Window activated events
            _ApplicationObject.Events.WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
            _ApplicationObject.Events.WindowEvents.WindowClosing -= WindowEvents_WindowClosing;
            _ApplicationObject.Events.WindowEvents.WindowCreated -= WindowEvents_WindowCreated;
            _ApplicationObject.Events.WindowEvents.WindowMoved -= WindowEvents_WindowMoved;

            // Build events
            _BuildEvents.OnBuildBegin -= BuildEvents_OnBuildBegin;
            _BuildEvents.OnBuildProjConfigDone -= BuildEvents_OnBuildProjConfigDone;
            _BuildEvents.OnBuildDone -= BuildEvents_OnBuildDone;
            _BuildEvents.OnBuildProjConfigBegin -= BuildEvents_OnBuildProjConfigBegin;

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnDisconnection(disconnectMode, ref custom);
            }
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnAddInsUpdate(ref Array custom)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnAddInsUpdate(ref custom);
            }
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnStartupComplete(ref Array custom)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnStartupComplete(ref custom);
            }
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnBeginShutdown(ref Array custom)
        {
            MyTickAlive();

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnBeginShutdown(ref custom);
            }
        }
        #endregion

        #region IDisposable methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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

        #region My methods
        private void MyOnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_TimerLock)
            {
                if (_IsActive)
                {
                    _IsInactiveNotified = false;
                }

                // No MyTickAlive() execution between timer elapsed method calls means that user is not active
                if (!_IsActive && !_IsInactiveNotified)
                {
                    foreach (var Sensor in _Sensors)
                    {
                        Sensor.OnUserInactive();
                    }

                    _IsInactiveNotified = true;
                }

                // On each timer elapse set active flag to false
                _IsActive = false;
            }
        }

        private void MyTickAlive()
        {
            lock (_TimerLock)
            {
                if (_IsActive)
                {
                    _IsActiveNotified = false;
                }

                if (!_IsActive && !_IsActiveNotified && _IsInactiveNotified)
                {
                    foreach (var Sensor in _Sensors)
                    {
                        Sensor.OnUserActiveAgain();
                    }

                    _IsActiveNotified = true;
                }

                _IsActive = true;
            }
        }
        #endregion
    }
}
