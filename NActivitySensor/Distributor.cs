namespace NActivitySensor
{
    #region Usings
    using EnvDTE;
    using EnvDTE80;
    using Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    #endregion

    /// <summary>
    /// Distributes events to the sensors
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Distributor : IDisposable
    {
        #region Private constants
        private const string _ConstConfigFileName = "NActivitySensor.config";
        #endregion

        #region DTE private variables
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
        #endregion

        #region Private variables
        private int _NumberOfSecondsToSetInactive = 10;

        private System.Timers.Timer _UserActivityTimer;
        private static object _UserActivityTimerLock = new object();

        private int _ProcessId;
        private IEnumerable<IActivitySensor> _Sensors;
        private readonly IConnectContext _ConnectContext;
        private readonly ILogger _Logger;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Distributor"/> class.
        /// </summary>
        /// <param name="sensors">The sensors.</param>
        /// <exception cref="System.ArgumentNullException">sensors</exception>
        public Distributor(IEnumerable<IActivitySensor> sensors, IConnectContext context, ILogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (sensors == null)
            {
                throw new ArgumentNullException("sensors");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _Sensors = sensors;
            _ConnectContext = context;
            _Logger = logger;

            _ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            foreach (var Sensor in _Sensors)
            {
                Sensor.OnConnect(_ProcessId);
            }

            MyTickAlive();
        }
        #endregion

        #region Solution events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_Opened()
        {
            try
            {
                MyTickAlive();
                TryOpenConfig();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionOpened();
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TryOpenConfig()
        {
            try
            {
                var SolutionDirectory = Path.GetDirectoryName(_ApplicationObject.Solution.FullName);
                string ConfigFilePath = Path.Combine(SolutionDirectory, _ConstConfigFileName);

                if (File.Exists(ConfigFilePath))
                {
                    ExeConfigurationFileMap ConfigMap = new ExeConfigurationFileMap();
                    ConfigMap.ExeConfigFilename = ConfigFilePath;
                    _ConnectContext.CurrentSolutionConfiguration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(ConfigMap, ConfigurationUserLevel.None);
                }
            }
            catch(Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_BeforeClosing()
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionBeforeClosing();
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Build events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void BuildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnBuildProjConfigDone(project, projectConfig, platform, solutionConfig, success);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnBuildDone(scope, action);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnBuildBegin(scope, action);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void BuildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnBuildProjConfigBegin(project, projectConfig, platform, solutionConfig);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Window events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void WindowEvents_WindowMoved(Window window, int top, int left, int width, int height)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowMoved(window, top, left, width, height);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void WindowEvents_WindowCreated(Window window)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowCreated(window);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void WindowEvents_WindowClosing(Window window)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowClosing(window);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void WindowEvents_WindowActivated(Window gotFocus, Window lostFocus)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowActivated(gotFocus, lostFocus);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region TextEditor events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            try
            {
                // Tick only when changes in scope of documents.
                // LineChanged event also occurs after adding text into output window.
                if (startPoint.Parent != null && startPoint.Parent.Parent != null)
                {
                    MyTickAlive();
                }


                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnLineChanged(startPoint, endPoint, hint);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region TaskList events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TaskListEvents_TaskRemoved(TaskItem taskItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnTaskRemoved(taskItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TaskListEvents_TaskNavigated(TaskItem taskItem, ref bool navigateHandled)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnTaskNavigated(taskItem, ref navigateHandled);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TaskListEvents_TaskModified(TaskItem taskItem, vsTaskListColumn columnModified)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnTaskModified(taskItem, columnModified);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void TaskListEvents_TaskAdded(TaskItem taskItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnTaskAdded(taskItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Solution items events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionItemsEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemRenamed(projectItem, oldName);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionItemsEvents_ItemRemoved(ProjectItem projectItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemRemoved(projectItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemAdded(ProjectItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Solution events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_Renamed(string oldName)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionRenamed(oldName);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionQueryClose(ref fCancel);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_ProjectRenamed(Project project, string oldName)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionProjectRenamed(project, oldName);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_ProjectRemoved(Project project)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionProjectRemoved(project);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_ProjectAdded(Project project)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionProjectAdded(project);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SolutionEvents_AfterClosing()
        {
            try
            {
                MyTickAlive();
                _ConnectContext.CurrentSolutionConfiguration = null;

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSolutionAfterClosing();
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void SelectionEvents_OnChange()
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnSelectionChange();
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Output window events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OutputWindowEvents_PaneUpdated(OutputWindowPane pPane)
        {
            try
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowPaneAdded(pPane);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OutputWindowEvents_PaneClearing(OutputWindowPane pPane)
        {
            try
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowPaneClearing(pPane);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OutputWindowEvents_PaneAdded(OutputWindowPane pPane)
        {
            try
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnWindowPaneAdded(pPane);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Files events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void MiscFilesEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemRenamed(projectItem, oldName);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void MiscFilesEvents_ItemRemoved(ProjectItem projectItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemRemoved(projectItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void MiscFilesEvents_ItemAdded(ProjectItem projectItem)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFileItemAdded(projectItem);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Find events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void FindEvents_FindDone(vsFindResult result, bool cancelled)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnFindDone(result, cancelled);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Debugger events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnExceptionThrown(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerExceptionThrown(exceptionType, name, code, description, ref exceptionAction);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnExceptionNotHandled(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerExceptionNotHandled(exceptionType, name, code, description, ref exceptionAction);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnEnterRunMode(dbgEventReason reason)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerEnterRunMode(reason);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnEnterDesignMode(dbgEventReason reason)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerEnterDesignMode(reason);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerEnterBreakMode(reason, ref executionAction);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void DebuggerEvents_OnContextChanged(EnvDTE.Process newProcess, Program newProgram, Thread newThread, EnvDTE.StackFrame newStackFrame)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDebuggerContextChanged(newProcess, newProgram, newThread, newStackFrame);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Command events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void CommandEvents_BeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            try
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnCommandBeforeExecute(guid, id, customIn, customOut, ref cancelDefault);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void CommandEvents_AfterExecute(string guid, int id, object customIn, object customOut)
        {
            try
            {
                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnCommandAfterExecute(guid, id, customIn, customOut);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Document events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OnDocumentClosing(Document document)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDocumentClosing(document);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OnDocumentSaved(Document document)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDocumentSaved(document);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void OnDocumentOpened(Document document)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnDocumentOpened(document);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region Methods
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "addInInst"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "custom"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "connectMode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst")]
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                _UserActivityTimer.Elapsed += MyOnUserActivityTimerElapsed;
                _UserActivityTimer.Start();

                _ApplicationObject = (DTE2)application;
                _Events = _ApplicationObject.Events;
                _DocumentEvents = _Events.DocumentEvents;
                _CommandEvents = _Events.CommandEvents;
                _DebuggerEvents = _Events.DebuggerEvents;
                _DocumentEvents = _Events.DocumentEvents;
                _FindEvents = _Events.FindEvents;
                _MiscFilesEvents = _Events.MiscFilesEvents;
                _OutputWindowEvents = _Events.OutputWindowEvents;
                _SelectionEvents = _Events.SelectionEvents;
                _SolutionEvents = _Events.SolutionEvents;
                _SolutionItemsEvents = _Events.SolutionItemsEvents;
                _TaskListEvents = _Events.TaskListEvents;
                _TextEditorEvents = _Events.TextEditorEvents;
                _WindowEvents = _Events.WindowEvents;
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
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                MyTickAlive();

                // Documents events
                _DocumentEvents.DocumentClosing -= OnDocumentClosing;
                _DocumentEvents.DocumentSaved -= OnDocumentSaved;
                _DocumentEvents.DocumentOpened -= OnDocumentOpened;

                // Command events
                _CommandEvents.AfterExecute -= CommandEvents_AfterExecute;
                _CommandEvents.BeforeExecute -= CommandEvents_BeforeExecute;

                // Debugger events
                _DebuggerEvents.OnContextChanged -= DebuggerEvents_OnContextChanged;
                _DebuggerEvents.OnEnterBreakMode -= DebuggerEvents_OnEnterBreakMode;
                _DebuggerEvents.OnEnterDesignMode -= DebuggerEvents_OnEnterDesignMode;
                _DebuggerEvents.OnEnterRunMode -= DebuggerEvents_OnEnterRunMode;
                _DebuggerEvents.OnExceptionNotHandled -= DebuggerEvents_OnExceptionNotHandled;
                _DebuggerEvents.OnExceptionThrown -= DebuggerEvents_OnExceptionThrown;

                // Find events
                _FindEvents.FindDone -= FindEvents_FindDone;

                // Misc files events
                _MiscFilesEvents.ItemAdded -= MiscFilesEvents_ItemAdded;
                _MiscFilesEvents.ItemRemoved -= MiscFilesEvents_ItemRemoved;
                _MiscFilesEvents.ItemRenamed -= MiscFilesEvents_ItemRenamed;

                // Output window events
                _OutputWindowEvents.PaneAdded -= OutputWindowEvents_PaneAdded;
                _OutputWindowEvents.PaneClearing -= OutputWindowEvents_PaneClearing;
                _OutputWindowEvents.PaneUpdated -= OutputWindowEvents_PaneUpdated;

                // Selection events
                _SelectionEvents.OnChange -= SelectionEvents_OnChange;

                // Solution events
                _SolutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
                _SolutionEvents.BeforeClosing -= SolutionEvents_BeforeClosing;
                _SolutionEvents.Opened -= SolutionEvents_Opened;
                _SolutionEvents.ProjectAdded -= SolutionEvents_ProjectAdded;
                _SolutionEvents.ProjectRemoved -= SolutionEvents_ProjectRemoved;
                _SolutionEvents.ProjectRenamed -= SolutionEvents_ProjectRenamed;
                _SolutionEvents.QueryCloseSolution -= SolutionEvents_QueryCloseSolution;
                _SolutionEvents.Renamed -= SolutionEvents_Renamed;

                // Solution items events
                _SolutionItemsEvents.ItemAdded -= SolutionItemsEvents_ItemAdded;
                _SolutionItemsEvents.ItemRemoved -= SolutionItemsEvents_ItemRemoved;
                _SolutionItemsEvents.ItemRenamed -= SolutionItemsEvents_ItemRenamed;

                // Task list events
                _TaskListEvents.TaskAdded -= TaskListEvents_TaskAdded;
                _TaskListEvents.TaskModified -= TaskListEvents_TaskModified;
                _TaskListEvents.TaskNavigated -= TaskListEvents_TaskNavigated;
                _TaskListEvents.TaskRemoved -= TaskListEvents_TaskRemoved;

                // Text editor events
                _TextEditorEvents.LineChanged -= TextEditorEvents_LineChanged;

                // Window activated events
                _WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
                _WindowEvents.WindowClosing -= WindowEvents_WindowClosing;
                _WindowEvents.WindowCreated -= WindowEvents_WindowCreated;
                _WindowEvents.WindowMoved -= WindowEvents_WindowMoved;

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
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnAddInsUpdate(ref Array custom)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnAddInsUpdate(ref custom);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnStartupComplete(ref Array custom)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnStartupComplete(ref custom);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public void OnBeginShutdown(ref Array custom)
        {
            try
            {
                MyTickAlive();

                foreach (var Sensor in _Sensors)
                {
                    Sensor.OnBeginShutdown(ref custom);
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
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
                if (_UserActivityTimer != null)
                {
                    _UserActivityTimer.Dispose();
                    _UserActivityTimer = null;
                }
            }
        }
        #endregion

        #region My methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void MyOnUserActivityTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_UserActivityTimerLock)
                {
                    if (_UserActivityTimer.Enabled)
                    {
                        // Notify sensors on user inactive
                        foreach (var Sensor in _Sensors)
                        {
                            Sensor.OnUserInactive();
                        }

                        // Disable timer
                        _UserActivityTimer.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void MyTickAlive()
        {
            lock (_UserActivityTimerLock)
            {
                try
                {
                    // If timer does not exists yet
                    if (_UserActivityTimer == null)
                    {
                        _UserActivityTimer = new System.Timers.Timer();
                        _UserActivityTimer.Elapsed += MyOnUserActivityTimerElapsed;
                    }

                    // Notify if timer is enabling after elapsed
                    if (!_UserActivityTimer.Enabled)
                    {
                        foreach (var Sensor in _Sensors)
                        {
                            Sensor.OnUserActive();
                        }
                    }

                    // Set timer on each tick
                    _UserActivityTimer.Interval = _NumberOfSecondsToSetInactive * 1000;
                    _UserActivityTimer.Enabled = true;
                }
                catch (Exception exception)
                {
                    _Logger.Log(exception);
                }
            }
        }
        #endregion
    }
}
