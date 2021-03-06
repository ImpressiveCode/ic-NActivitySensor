﻿namespace NActivitySensor
{
    #region Usings
    using System;

    #endregion

    public sealed class NullActivitySensor : IActivitySensor
    {
        #region Private variables
        private static NullActivitySensor _Instance = null;
        #endregion

        #region Constructors
        private NullActivitySensor()
        {
        }
        #endregion

        #region Properties
        public static NullActivitySensor Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new NullActivitySensor();
                }

                return _Instance;
            }
        }
        #endregion

        #region IActivitySensor methods
        public void OnSolutionAfterClosing()
        {
            
        }

        public void OnSolutionOpened()
        {
            
        }

        public void OnSolutionBeforeClosing()
        {
            
        }

        public void OnSolutionRenamed(string oldName)
        {
            
        }

        public void OnSolutionQueryClose(ref bool fCancel)
        {
            
        }

        public void OnSolutionProjectRenamed(EnvDTE.Project project, string oldName)
        {
            
        }

        public void OnSolutionProjectRemoved(EnvDTE.Project project)
        {
            
        }

        public void OnSolutionProjectAdded(EnvDTE.Project project)
        {
            
        }

        public void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            
        }

        public void OnBuildDone(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            
        }

        public void OnBuildBegin(EnvDTE.vsBuildScope scope, EnvDTE.vsBuildAction action)
        {
            
        }

        public void OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            
        }

        public void OnUserInactive()
        {
            
        }

        public void OnUserActive()
        {
            
        }

        public void OnConnect(int processId)
        {
            
        }

        public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            
        }

        public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {
            
        }

        public void OnAddInsUpdate(ref Array custom)
        {
            
        }

        public void OnStartupComplete(ref Array custom)
        {
            
        }

        public void OnBeginShutdown(ref Array custom)
        {
            
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
    }
}
