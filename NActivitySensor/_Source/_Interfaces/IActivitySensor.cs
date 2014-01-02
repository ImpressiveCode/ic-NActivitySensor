using EnvDTE;
using Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public interface IActivitySensor
    {
        #region Solution events
        void OnSolutionAfterClosing(string solutionFullName);
        void OnSolutionOpened(string solutionFullName);
        void OnSolutionBeforeClosing(string solutionFulName);
        void OnSolutionRenamed(string oldName);
        void OnSolutionQueryClose(ref bool fCancel);
        void OnSolutionProjectRenamed(Project project, string oldName);
        void OnSolutionProjectRemoved(Project project);
        void OnSolutionProjectAdded(Project project);
        #endregion

        #region Build events
        void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success);
        void OnBuildDone(vsBuildScope scope, vsBuildAction action);
        void OnBuildBegin(vsBuildScope scope, vsBuildAction action);
        void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig);
        #endregion

        #region User activity events
        void OnUserInactive();
        #endregion

        #region Plugin lifetime events
        void OnConnect(int processId);
        void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom);
        void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom);
        void OnAddInsUpdate(ref Array custom);
        void OnStartupComplete(ref Array custom);
        void OnBeginShutdown(ref Array custom);
        #endregion

        #region Window events
        void OnWindowMoved(Window window, int top, int left, int width, int height);
        void OnWindowCreated(Window window);
        void OnWindowClosing(Window window);
        void OnWindowActivated(Window gotFocus, Window lostFocus);
        #endregion

        #region Output window events
        void OnWindowPaneUpdated(OutputWindowPane pPane);
        void OnWindowPaneClearing(OutputWindowPane pPane);
        void OnWindowPaneAdded(OutputWindowPane pPane);
        #endregion

        #region Selection events
        void OnSelectionChange();
        #endregion

        #region Editor events
        void OnLineChanged(TextPoint startPoint, TextPoint endPoint, int hint);
        #endregion

        #region Task events
        void OnTaskRemoved(TaskItem taskItem);
        void OnTaskNavigated(TaskItem taskItem, ref bool navigateHandled);
        void OnTaskModified(TaskItem taskItem, vsTaskListColumn columnModified);
        void OnTaskAdded(TaskItem taskItem);
        #endregion

        #region File item events
        void OnFileItemRenamed(ProjectItem projectItem, string oldName);
        void OnFileItemRemoved(ProjectItem projectItem);
        void OnFileItemAdded(ProjectItem projectItem);
        #endregion

        #region Find events
        void OnFindDone(vsFindResult result, bool cancelled);
        #endregion

        #region Debugger events
        void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction);
        void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction);
        void OnDebuggerEnterRunMode(dbgEventReason reason);
        void OnDebuggerEnterDesignMode(dbgEventReason reason);
        void OnDebuggerEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction);
        void OnDebuggerContextChanged(EnvDTE.Process newProcess, Program newProgram, Thread newThread, EnvDTE.StackFrame newStackFrame);
        #endregion

        #region Command events
        void OnCommandBeforeExecute(string guid, int iD, object customIn, object customOut, ref bool cancelDefault);
        void OnCommandAfterExecute(string guid, int iD, object customIn, object customOut);
        #endregion

        #region Document events
        void OnDocumentClosing(Document Document);
        void OnDocumentSaved(Document Document);
        void OnDocumentOpened(Document Document);
        #endregion
    }
}
