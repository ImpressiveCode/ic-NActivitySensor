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
    public class DTEActivitySensor : BaseActivitySensor
    {
        #region Private variables
        private readonly IEnumerable<IReporter> _Reporters;
        private readonly IReportContentSerializer _ReportContentSerializer;
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

        #region IActivitySensor methods
        public override void OnBuildDone(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            try
            {
                var Report = new Report(new BuildReportContent
                {
                    Action = action.ToString(),
                    Scope = scope.ToString()
                }, SensorBuildEvent.BuildDone.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
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
                }, SensorBuildEvent.BuildProjConfigDone.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnBuildBegin(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            try
            {
                var Report = new Report(new BuildReportContent
                {
                    Scope = scope.ToString(),
                    Action = action.ToString()
                }, SensorBuildEvent.BuildBegin.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            try
            {
                var Report = new Report(new BuildProjConfigContent
                {
                    Project = new ProjectInfoContent() { Name = project },
                    ProjectConfig = projectConfig,
                    Platform = platform,
                    SolutionConfig = solutionConfig,
                }, SensorBuildEvent.BuildProjConfigBegin.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionAfterClosing()
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionAfterClosing.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionOpened()
        {
            try
            {
                List<ProjectInfoContent> FoundProjects = new List<ProjectInfoContent>();

                if (DTEApplication != null && DTEApplication.Solution != null)
                {
                    if (DTEApplication.Solution.Projects != null)
                    {
                        foreach (EnvDTE.Project LoopProject in DTEApplication.Solution.Projects)
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
                    SolutionName = SolutionName,
                    Projects = FoundProjects
                }, SensorSolutionEvent.SolutionOpened.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);

            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionBeforeClosing()
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionBeforeClosing.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionRenamed(string oldName)
        {
            try
            {
                var NewName = DTEApplication.Solution.FullName;

                var Report = new Report(new SolutionRenamedContent()
                {
                    NewName = NewName,
                    OldName = oldName
                }, SensorSolutionEvent.SolutionRenamed.ToString(), ProcessId, NewName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionQueryClose(ref bool fCancel)
        {
            try
            {
                var Report = new Report(new object(), SensorSolutionEvent.SolutionQueryClose.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionProjectRenamed(EnvDTE.Project project, string oldName)
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

                var Report = new Report(ProjectRenamedContent, SensorSolutionEvent.SolutionProjectRenamed.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionProjectRemoved(EnvDTE.Project project)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentNullException("project");
                }

                var ProjectInfo = MyCreateProjectInfo(project);

                var Report = new Report(ProjectInfo, SensorSolutionEvent.SolutionProjectRemoved.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnSolutionProjectAdded(EnvDTE.Project project)
        {
            try
            {
                if (project == null)
                {
                    throw new ArgumentNullException("project");
                }

                var ProjectInfo = MyCreateProjectInfo(project);

                var Report = new Report(ProjectInfo, SensorSolutionEvent.SolutionProjectAdded.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnUserInactive()
        {
            try
            {
                var Report = new Report(new object(), SensorUserEvent.UserInactive.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnUserActive()
        {
            try
            {
                var Report = new Report(new object(), SensorUserEvent.UserActiveAgain.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst")]
        public override void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            base.OnConnection(application, connectMode, addInInst, ref custom);

            try
            {
                List<string> ActiveWindows = new List<string>();
                foreach (EnvDTE.Window LoopWindow in DTEApplication.Windows)
                {
                    ActiveWindows.Add(LoopWindow.Caption);
                }

                var Content = new ConnectionContent()
                {
                    ActiveWindow = DTEApplication.ActiveWindow.Caption,
                    Edition = DTEApplication.Edition,
                    FullName = DTEApplication.FullName,
                    LocaleId = DTEApplication.LocaleID,
                    MainWindow = DTEApplication.MainWindow.Caption,
                    Mode = DTEApplication.Mode.ToString(),
                    Name = DTEApplication.Name,
                    Version = DTEApplication.Version,
                    Windows = ActiveWindows
                };

                var Report = new Report(Content, SensorPluginEvent.Connection.ToString(), ProcessId, SolutionName, _ReportContentSerializer);

                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public override void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.Disconnection.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnAddInsUpdate(ref Array custom)
        {

        }

        public override void OnStartupComplete(ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.StartupComplete.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnBeginShutdown(ref Array custom)
        {
            try
            {
                var Report = new Report(new object(), SensorPluginEvent.BeginShutdown.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }   

        public override void OnFileItemRenamed(EnvDTE.ProjectItem projectItem, string oldName)
        {
            try
            {
                var ProjectItemRenameContent = new ProjectItemRenamedContent()
                {
                    ProjectItem = MyCreateProjectItemInfo(projectItem),
                    OldName = oldName
                };

                var Report = new Report(ProjectItemRenameContent, SensorFileItemEvent.FileItemRenamed.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnFileItemRemoved(EnvDTE.ProjectItem projectItem)
        {
            try
            {
                var Report = new Report(MyCreateProjectItemInfo(projectItem), SensorFileItemEvent.FileItemRemoved.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnFileItemAdded(EnvDTE.ProjectItem projectItem)
        {
            try
            {
                var Report = new Report(MyCreateProjectItemInfo(projectItem), SensorFileItemEvent.FileItemAdded.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnFindDone(EnvDTE.vsFindResult result, bool canceled)
        {
            try
            {
                var Report = new Report(result, SensorFindEvent.FindDone.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
            try
            {
                var ExceptionContent = MyCreateDebuggerException(exceptionType, name, code, description, ref exceptionAction);
                var Report = new Report(ExceptionContent, SensorDebuggerEvent.DebuggerExceptionThrown.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {
            try
            {
                var ExceptionContent = MyCreateDebuggerException(exceptionType, name, code, description, ref exceptionAction);
                var Report = new Report(ExceptionContent, SensorDebuggerEvent.DebuggerExceptionNotHandled.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerEnterRunMode(EnvDTE.dbgEventReason reason)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterRunMode.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerEnterDesignMode(EnvDTE.dbgEventReason reason)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterDesignMode.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerEnterBreakMode(EnvDTE.dbgEventReason reason, ref EnvDTE.dbgExecutionAction executionAction)
        {
            try
            {
                var DebuggerModeContent = new DebuggerModeContent()
                {
                    Reason = reason.ToString(),
                    ExecutionAction = executionAction.ToString()
                };

                var Report = new Report(DebuggerModeContent, SensorDebuggerEvent.DebuggerEnterBreakMode.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDebuggerContextChanged(EnvDTE.Process newProcess, EnvDTE.Program newProgram, EnvDTE.Thread newThread, EnvDTE.StackFrame newStackFrame)
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

                var Report = new Report(DebuggerContextContent, SensorDebuggerEvent.DebuggerContextChanged.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDocumentClosing(EnvDTE.Document document)
        {
            try
            {
                var Report = new Report(MyCreateDocument(document), SensorDocumentEvent.DocumentClosing.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDocumentSaved(EnvDTE.Document document)
        {
            try
            {
                var Report = new Report(MyCreateDocument(document), SensorDocumentEvent.DocumentSaved.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
        }

        public override void OnDocumentOpened(EnvDTE.Document document)
        {
            try
            {
                var Report = new Report(MyCreateDocument(document), SensorDocumentEvent.DocumentOpened.ToString(), ProcessId, SolutionName, _ReportContentSerializer);
                MyReportAll(Report);
            }
            catch (Exception exception)
            {
                throw new SensorException(exception.Message, exception);
            }
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

        private DocumentContent MyCreateDocument(EnvDTE.Document document)
        {
            return new DocumentContent()
            {
                Name = document.Name,
                Kind = document.Kind,
                Path = document.Path
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
