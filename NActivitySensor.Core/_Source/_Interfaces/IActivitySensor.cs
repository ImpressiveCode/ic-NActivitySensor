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
        void OnSolutionBeforeClosing(string solutionFullName);
        void OnSolutionRenamed(string oldName);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void OnSolutionQueryClose(ref bool fCancel);
        void OnSolutionProjectRenamed(Project project, string oldName);
        void OnSolutionProjectRemoved(Project project);
        void OnSolutionProjectAdded(Project project);
        #endregion

        #region Build events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
        void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success);
        void OnBuildDone(vsBuildScope scope, vsBuildAction action);
        void OnBuildBegin(vsBuildScope scope, vsBuildAction action);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
        void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig);
        #endregion

        #region User activity events
        void OnUserInactive();
        #endregion

        #region Plugin lifetime events
        void OnConnect(int processId);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Inst")]
        void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void OnAddInsUpdate(ref Array custom);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void OnStartupComplete(ref Array custom);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void OnBeginShutdown(ref Array custom);
        #endregion

        #region Window events
        void OnWindowMoved(Window window, int top, int left, int width, int height);
        void OnWindowCreated(Window window);
        void OnWindowClosing(Window window);
        void OnWindowActivated(Window gotFocus, Window lostFocus);
        #endregion

        #region Output window events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WindowPane"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        void OnWindowPaneUpdated(OutputWindowPane pPane);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
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
        void OnFindDone(vsFindResult result, bool canceled);
        #endregion

        #region Debugger events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "4#")]
        void OnDebuggerExceptionThrown(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "4#")]
        void OnDebuggerExceptionNotHandled(string exceptionType, string name, int code, string description, ref dbgExceptionAction exceptionAction);
        void OnDebuggerEnterRunMode(dbgEventReason reason);
        void OnDebuggerEnterDesignMode(dbgEventReason reason);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        void OnDebuggerEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction);
        void OnDebuggerContextChanged(EnvDTE.Process newProcess, Program newProgram, Thread newThread, EnvDTE.StackFrame newStackFrame);
        #endregion

        #region Command events
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "4#")]
        void OnCommandBeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault);
        void OnCommandAfterExecute(string guid, int id, object customIn, object customOut);
        #endregion

        #region Document events
        void OnDocumentClosing(Document document);
        void OnDocumentSaved(Document document);
        void OnDocumentOpened(Document document);
        #endregion
    }
}
