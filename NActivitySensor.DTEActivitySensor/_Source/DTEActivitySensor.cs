namespace NActivitySensor
{
    #region Usings
    using EnvDTE80;
    using NActivitySensor.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DTE")]
    public class DTEActivitySensor : IActivitySensor
    {
        #region Private variables
        private readonly IEnumerable<IReporter> _Reporters;
        private readonly IReportContentSerializer _ReportContentSerializer;

        private DTE2 _Application;

        private string _SolutionFullName
        {
            get
            {
                if (_Application != null && _Application.Solution != null && _Application.Solution.FullName != null)
                {
                    return _Application.Solution.FullName;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        private string _SolutionSimpleName
        {
            get
            {
                var FileName = Path.GetFileNameWithoutExtension(_SolutionFullName);                
                return FileName;
            }
        }

        private int? _ProcessId = null;
        #endregion

        #region Constructors
        public DTEActivitySensor(IEnumerable<IReporter> reporters, IReportContentSerializer reportContentSerializer)
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

        #region IAcvititySensor implemented methods
        public void OnBuildDone(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            try
            {
                var Report = new Report(new BuildReportContent
                {
                    Action = action.ToString(),
                    Scope = scope.ToString()
                }, SensorBuildEvent.BuildDone.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            try
            {
                var Report = new Report(new BuildProjConfigContent
                {
                    Project = new ProjectInfoContent() { Name = project },
                    ProjectConfig = projectConfig,
                    Platform = platform,
                    SolutionConfig = solutionConfig,
                    Success = success
                }, SensorBuildEvent.BuildProjConfigDone.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnBuildBegin(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            try
            {
                var Report = new Report(new BuildReportContent
                {
                    Scope = scope.ToString(),
                    Action = action.ToString()
                }, SensorBuildEvent.BuildBegin.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            try
            {
                var Report = new Report(new BuildProjConfigContent
                {
                    Project = new ProjectInfoContent() { Name = project },
                    ProjectConfig = projectConfig,
                    Platform = platform,
                    SolutionConfig = solutionConfig,
                }, SensorBuildEvent.BuildProjConfigBegin.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion

        #region IActivitySensor methods
        public void OnSolutionAfterClosing()
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionAfterClosing.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionOpened()
        {
            try
            {
                List<ProjectInfoContent> FoundProjects = new List<ProjectInfoContent>();

                if (_Application != null && _Application.Solution != null)
                {
                    if (_Application.Solution.Projects != null)
                    {
                        foreach (EnvDTE.Project LoopProject in _Application.Solution.Projects)
                        {
                            string ProjectName = String.Empty;

                            if (LoopProject.UniqueName != null)
                            {
                                ProjectName = LoopProject.UniqueName;
                            }

                            FoundProjects.Add(new ProjectInfoContent() { Name = ProjectName });
                        }
                    }
                }

                var Report = new Report(new SolutionInfoContent
                {
                    SolutionName = _SolutionSimpleName,
                    Projects = FoundProjects
                }, SensorSolutionEvent.SolutionOpened.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);

            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionBeforeClosing()
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionBeforeClosing.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionRenamed(string oldName)
        {
            try
            {
                var NewName = _Application.Solution.FullName;

                var Report = new Report(new SolutionRenamedContent()
                {
                    NewName = NewName,
                    OldName = oldName
                }, SensorSolutionEvent.SolutionRenamed.ToString(), _ProcessId, NewName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionQueryClose(ref bool fCancel)
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionQueryClose.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionProjectRenamed(EnvDTE.Project project, string oldName)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentNullException("project");
                }

                var ProjectRenamedContent = new ProjectRenamedContent()
                {
                    Project = MyCreateProjectInfo(project),
                    OldName = oldName
                };

                var Report = new Report(ProjectRenamedContent, SensorSolutionEvent.SolutionProjectRenamed.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionProjectRemoved(EnvDTE.Project project)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentNullException("project");
                }

                var ProjectInfo = MyCreateProjectInfo(project);

                var Report = new Report(ProjectInfo, SensorSolutionEvent.SolutionProjectRemoved.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionProjectAdded(EnvDTE.Project project)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentNullException("project");
                }

                var ProjectInfo = MyCreateProjectInfo(project);

                var Report = new Report(ProjectInfo, SensorSolutionEvent.SolutionProjectAdded.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnUserInactive()
        {
            try
            {
                var Report = new Report(new object(), SensorUserEvent.UserInactive.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnUserActiveAgain()
        {
            try
            {
                var Report = new Report(new object(), SensorUserEvent.UserActiveAgain.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnConnect(int processId)
        {
            this._ProcessId = processId;
        }

        public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                _Application = (DTE2)application;

                List<string> ActiveWindows = new List<string>();
                foreach (EnvDTE.Window LoopWindow in _Application.Windows)
                {
                    ActiveWindows.Add(LoopWindow.Caption);
                }

                var Content = new ConnectionContent()
                {
                    ActiveWindow = _Application.ActiveWindow.Caption,
                    Edition = _Application.Edition,
                    FullName = _Application.FullName,
                    LocaleId = _Application.LocaleID,
                    MainWindow = _Application.MainWindow.Caption,
                    Mode = _Application.Mode.ToString(),
                    Name = _Application.Name,
                    Version = _Application.Version,
                    Windows = ActiveWindows
                };

                var Report = new Report(Content, SensorPluginEvent.Connection.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.Disconnection.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnAddInsUpdate(ref Array custom)
        {

        }

        public void OnStartupComplete(ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.StartupComplete.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnBeginShutdown(ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.BeginShutdown.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnWindowMoved(EnvDTE.Window window, int top, int left, int width, int height)
        {

        }

        public void OnWindowCreated(EnvDTE.Window window)
        {

        }

        public void OnWindowClosing(EnvDTE.Window window)
        {

        }

        public void OnWindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
        {

        }

        public void OnWindowPaneUpdated(EnvDTE.OutputWindowPane pPane)
        {

        }

        public void OnWindowPaneClearing(EnvDTE.OutputWindowPane pPane)
        {

        }

        public void OnWindowPaneAdded(EnvDTE.OutputWindowPane pPane)
        {

        }

        public void OnSelectionChange()
        {
            try
            {
                var Report = new Report(new object(), SensorSelectionEvent.SelectionChange.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnLineChanged(EnvDTE.TextPoint startPoint, EnvDTE.TextPoint endPoint, int hint)
        {
            
        }

        public void OnTaskRemoved(EnvDTE.TaskItem taskItem)
        {

        }

        public void OnTaskNavigated(EnvDTE.TaskItem taskItem, ref bool navigateHandled)
        {

        }

        public void OnTaskModified(EnvDTE.TaskItem taskItem, EnvDTE.vsTaskListColumn columnModified)
        {

        }

        public void OnTaskAdded(EnvDTE.TaskItem taskItem)
        {

        }

        public void OnFileItemRenamed(EnvDTE.ProjectItem projectItem, string oldName)
        {
            try
            {
                var ProjectItemRenameContent = new ProjectItemRenamedContent()
                {
                    ProjectItem = MyCreateProjectItemInfo(projectItem),
                    OldName = oldName
                };

                var Report = new Report(ProjectItemRenameContent, SensorSelectionEvent.SelectionChange.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnFileItemRemoved(EnvDTE.ProjectItem projectItem)
        {
            try
            {
                var Report = new Report(MyCreateProjectItemInfo(projectItem), SensorSelectionEvent.SelectionChange.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnFileItemAdded(EnvDTE.ProjectItem projectItem)
        {
            try
            {
                var Report = new Report(MyCreateProjectItemInfo(projectItem), SensorSelectionEvent.SelectionChange.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnFindDone(EnvDTE.vsFindResult result, bool canceled)
        {
            try
            {
                var Report = new Report(result, SensorFindEvent.FindDone.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
            try
            {
                var ExceptionContent = MyCreateDebuggerException(exceptionType, name, code, description, ref exceptionAction);
                var Report = new Report(ExceptionContent, SensorDebuggerEvent.DebuggerExceptionThrown.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
            try
            {
                var ExceptionContent = MyCreateDebuggerException(exceptionType, name, code, description, ref exceptionAction);
                var Report = new Report(ExceptionContent, SensorDebuggerEvent.DebuggerExceptionNotHandled.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerEnterRunMode(EnvDTE.dbgEventReason reason)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterRunMode.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerEnterDesignMode(EnvDTE.dbgEventReason reason)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterDesignMode.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerEnterBreakMode(EnvDTE.dbgEventReason reason, ref EnvDTE.dbgExecutionAction executionAction)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString(),
                    ExecutionAction = executionAction.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterBreakMode.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnDebuggerContextChanged(EnvDTE.Process newProcess, EnvDTE.Program newProgram, EnvDTE.Thread newThread, EnvDTE.StackFrame newStackFrame)
        {
            try
            {
                if (newProcess == null)
                {
                    throw new ArgumentNullException("newProcess");
                }

                if (newProgram == null)
                {
                    throw new ArgumentNullException("newProgram");
                }

                if (newThread == null)
                {
                    throw new ArgumentNullException("newThread");
                }

                if (newStackFrame == null)
                {
                    throw new ArgumentNullException("newStackFrame");
                }

                var DebuggerContextContent = new DebuggerContextChanged()
                {
                    ProcessId = newProcess.ProcessID,
                    ProcessName = newProcess.Name,
                    ProgramName = newProgram.Name,
                    ProgramProcessId = newProgram.Process.ProcessID,
                    ThreadId = newThread.ID,
                    ThreadName = newThread.Name,
                    StackFrameFunctionName = newStackFrame.FunctionName
                };

                var Report = new Report(DebuggerContextContent, SensorDebuggerEvent.DebuggerContextChanged.ToString(), _ProcessId, _SolutionSimpleName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnCommandBeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {

        }

        public void OnCommandAfterExecute(string guid, int id, object customIn, object customOut)
        {

        }

        public void OnDocumentClosing(EnvDTE.Document document)
        {

        }

        public void OnDocumentSaved(EnvDTE.Document document)
        {

        }

        public void OnDocumentOpened(EnvDTE.Document document)
        {

        }
        #endregion

        #region My methods
        private ProjectItemInfoContent MyCreateProjectItemInfo(EnvDTE.ProjectItem projectItem)
        {
            return new ProjectItemInfoContent()
            {
                Name = projectItem.Name
            };
        }

        private ProjectInfoContent MyCreateProjectInfo(EnvDTE.Project project)
        {
            return new ProjectInfoContent()
            {
                Name = project.UniqueName
            };
        }

        private DebuggerExceptionContent MyCreateDebuggerException(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
            return new DebuggerExceptionContent()
            {
                Code = code,
                Description = description,
                ExceptionAction = exceptionAction.ToString(),
                ExceptionType = exceptionType,
                Name = name
            };
        }

        private void MyReportAll(Report report)
        {
            foreach (var LoopReporter in _Reporters)
            {
                LoopReporter.Report(report);
            }
        }
        #endregion
    }
}
