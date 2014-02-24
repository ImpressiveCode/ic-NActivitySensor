namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public abstract class BaseActivitySensor : IActivitySensor
    {
        #region IActivitySensor methods
        public virtual void OnSolutionAfterClosing()
        {
        }

        public virtual void OnSolutionOpened()
        {

        }

        public virtual void OnSolutionBeforeClosing()
        {

        }

        public virtual void OnSolutionRenamed(string oldName)
        {

        }

        public virtual void OnSolutionQueryClose(ref bool fCancel)
        {

        }

        public virtual void OnSolutionProjectRenamed(EnvDTE.Project project, string oldName)
        {

        }

        public virtual void OnSolutionProjectRemoved(EnvDTE.Project project)
        {

        }

        public virtual void OnSolutionProjectAdded(EnvDTE.Project project)
        {

        }

        public virtual void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {

        }

        public virtual void OnBuildDone(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {

        }

        public virtual void OnBuildBegin(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {

        }

        public virtual void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {

        }

        public virtual void OnUserInactive()
        {

        }

        public virtual void OnUserActive()
        {

        }

        public virtual void OnConnect(int processId)
        {

        }

        public virtual void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {

        }

        public virtual void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {

        }

        public virtual void OnAddInsUpdate(ref Array custom)
        {

        }

        public virtual void OnStartupComplete(ref Array custom)
        {

        }

        public virtual void OnBeginShutdown(ref Array custom)
        {

        }

        public virtual void OnWindowMoved(EnvDTE.Window window, int top, int left, int width, int height)
        {

        }

        public virtual void OnWindowCreated(EnvDTE.Window window)
        {

        }

        public virtual void OnWindowClosing(EnvDTE.Window window)
        {

        }

        public virtual void OnWindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
        {

        }

        public virtual void OnWindowPaneUpdated(EnvDTE.OutputWindowPane pPane)
        {

        }

        public virtual void OnWindowPaneClearing(EnvDTE.OutputWindowPane pPane)
        {

        }

        public virtual void OnWindowPaneAdded(EnvDTE.OutputWindowPane pPane)
        {

        }

        public virtual void OnSelectionChange()
        {

        }

        public virtual void OnLineChanged(EnvDTE.TextPoint startPoint, EnvDTE.TextPoint endPoint, int hint)
        {

        }

        public virtual void OnTaskRemoved(EnvDTE.TaskItem taskItem)
        {

        }

        public virtual void OnTaskNavigated(EnvDTE.TaskItem taskItem, ref bool navigateHandled)
        {

        }

        public virtual void OnTaskModified(EnvDTE.TaskItem taskItem, EnvDTE.vsTaskListColumn columnModified)
        {

        }

        public virtual void OnTaskAdded(EnvDTE.TaskItem taskItem)
        {

        }

        public virtual void OnFileItemRenamed(EnvDTE.ProjectItem projectItem, string oldName)
        {

        }

        public virtual void OnFileItemRemoved(EnvDTE.ProjectItem projectItem)
        {

        }

        public virtual void OnFileItemAdded(EnvDTE.ProjectItem projectItem)
        {

        }

        public virtual void OnFindDone(EnvDTE.vsFindResult result, bool canceled)
        {

        }

        public virtual void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {

        }

        public virtual void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref EnvDTE.dbgExceptionAction exceptionAction)
        {

        }

        public virtual void OnDebuggerEnterRunMode(EnvDTE.dbgEventReason reason)
        {

        }

        public virtual void OnDebuggerEnterDesignMode(EnvDTE.dbgEventReason reason)
        {

        }

        public virtual void OnDebuggerEnterBreakMode(EnvDTE.dbgEventReason reason, ref EnvDTE.dbgExecutionAction executionAction)
        {

        }

        public virtual void OnDebuggerContextChanged(EnvDTE.Process newProcess, EnvDTE.Program newProgram, EnvDTE.Thread newThread, EnvDTE.StackFrame newStackFrame)
        {

        }

        public virtual void OnCommandBeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {

        }

        public virtual void OnCommandAfterExecute(string guid, int id, object customIn, object customOut)
        {

        }

        public virtual void OnDocumentClosing(EnvDTE.Document document)
        {

        }

        public virtual void OnDocumentSaved(EnvDTE.Document document)
        {

        }

        public virtual void OnDocumentOpened(EnvDTE.Document document)
        {

        }
        #endregion    
    }
}
