namespace NActivitySensor
{
    #region Usings
    using EnvDTE80;
    using NActivitySensor.Models;
    using System;
    using System.Collections.Generic;

    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DTE")]
    public class DTEActivitySensor : IActivitySensor
    {
        #region Private variables
        private readonly IEnumerable<IReporter> _Reporters;
        private readonly IReportContentSerializer _ReportContentSerializer;

        private DTE2 _Application;
        private string _SolutionFullName = String.Empty;
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
                }, SensorBuildEvent.BuildDone.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

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
                    Project = project,
                    ProjectConfig = projectConfig,
                    Platform = platform,
                    SolutionConfig = solutionConfig,
                    Success = success
                }, SensorBuildEvent.BuildProjConfigDone.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

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
                }, SensorBuildEvent.BuildBegin.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

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
                    Project = project,
                    ProjectConfig = projectConfig,
                    Platform = platform,
                    SolutionConfig = solutionConfig,
                }, SensorBuildEvent.BuildProjConfigBegin.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }
        #endregion

        #region IActivitySensor methods
        public void OnSolutionAfterClosing(string solutionFullName)
        {
            try
            {
                if (solutionFullName != null)
                {
                    _SolutionFullName = solutionFullName;
                }

                var Report = new Report(new object(), SensorSolutionEvent.SolutionAfterClosing.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionOpened(string solutionFullName)
        {
            try
            {
                if (solutionFullName != null)
                {
                    _SolutionFullName = solutionFullName;
                }

                List<string> FoundProjects = new List<string>();
                if (_Application != null && _Application.Solution != null)
                {
                    if (_Application.Solution.Projects != null)
                    {
                        foreach (EnvDTE.Project LoopProject in _Application.Solution.Projects)
                        {
                            string ProjectName = String.Empty;

                            if (LoopProject.Name != null && LoopProject.Name is string)
                            {
                                ProjectName = LoopProject.Name;
                            }

                            FoundProjects.Add(ProjectName);
                        }
                    }
                }

                var Report = new Report(new SolutionInfoContent
                {
                    SolutionName = _SolutionFullName,
                    Projects = FoundProjects
                }, SensorSolutionEvent.SolutionOpened.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
                MyReportAll(Report);

            }
            catch (Exception exception)
            {
                throw new ReporterException(exception.Message, exception);
            }
        }

        public void OnSolutionBeforeClosing(string solutionFullName)
        {
            try
            {
                if (solutionFullName != null)
                {
                    _SolutionFullName = solutionFullName;
                }

                var Report = new Report(new object(), SensorSolutionEvent.SolutionBeforeClosing.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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
                var Report = new Report(new object(), SensorSolutionEvent.SolutionQueryClose.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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

                var Report = new Report(project.ToString(), SensorSolutionEvent.SolutionProjectRenamed.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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

                var Report = new Report(project.ToString(), SensorSolutionEvent.SolutionProjectRemoved.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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

                var Report = new Report(project.ToString(), SensorSolutionEvent.SolutionProjectAdded.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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
                var Report = new Report(new object(), SensorUserEvent.UserInactive.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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

                string CurrentSolution = String.Empty;
                if (_Application != null && _Application.Solution != null && _Application.Solution.FullName != null)
                {
                    CurrentSolution = _Application.Solution.FullName;
                }

                var Report = new Report(new object(), SensorPluginEvent.Connection.ToString(), _ProcessId, CurrentSolution, _ReportContentSerializer);

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
                var Report = new Report(new object(), SensorPluginEvent.Disconnection.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);

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
                var Report = new Report(new object(), SensorPluginEvent.StartupComplete.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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
                var Report = new Report(new object(), SensorPluginEvent.BeginShutdown.ToString(), _ProcessId, _SolutionFullName, _ReportContentSerializer);
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

        }

        public void OnFileItemRemoved(EnvDTE.ProjectItem projectItem)
        {

        }

        public void OnFileItemAdded(EnvDTE.ProjectItem projectItem)
        {

        }

        public void OnFindDone(EnvDTE.vsFindResult result, bool canceled)
        {

        }

        public void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
        }

        public void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {

        }

        public void OnDebuggerEnterRunMode(EnvDTE.dbgEventReason reason)
        {

        }

        public void OnDebuggerEnterDesignMode(EnvDTE.dbgEventReason reason)
        {

        }

        public void OnDebuggerEnterBreakMode(EnvDTE.dbgEventReason reason, ref EnvDTE.dbgExecutionAction executionAction)
        {

        }

        public void OnDebuggerContextChanged(EnvDTE.Process newProcess, EnvDTE.Program newProgram, EnvDTE.Thread newThread, EnvDTE.StackFrame newStackFrame)
        {

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
