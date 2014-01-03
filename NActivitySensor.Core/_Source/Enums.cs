namespace NActivitySensor
{
    public enum SensorSolutionEvent
    {
        SolutionAfterClosing,
        SolutionOpened,
        SolutionBeforeClosing,
        SolutionRenamed,
        SolutionQueryClose,
        SolutionProjectRenamed,
        SolutionProjectRemoved,
        SolutionProjectAdded
    }

    public enum SensorBuildEvent
    {
        BuildDone,
        BuildProjConfigDone,
        BuildBegin,
        BuildProjConfigBegin
    }

    public enum SensorUserEvent
    {
        UserInactive
    }

    public enum SensorPluginEvent
    {
        Connect,
        Connection,
        Disconnection,
        AddInsUpdate,
        StartupComplete,
        BeginShutdown
    }

    public enum SensorWindowEvent
    {
        WindowMoved,
        WindowCreated,
        WindowClosing,
        WindowActivated,
        WindowPaneUpdated,
        WindowPaneClearing,
        WindowPaneAdded
    }

    public enum SensorSelectionEvent
    {
        SelectionChange
    }

    public enum SensorTextEditorEvent
    {
        LineChanged
    }

    public enum SensorTaskEvent
    {
        TaskRemoved,
        TaskNavigated,
        TaskModified,
        TaskAdded,
    }

    public enum SensorFileItemEvent
    {
        FileItemRenamed,
        FileItemRemoved,
        FileItemAdded,
    }

    public enum SensorFindEvent
    {
        FindDone
    }

    public enum SensorDebuggerEvent
    {
        DebuggerExceptionThrown,
        DebuggerExceptionNotHandled,
        DebuggerEnterRunMode,
        DebuggerEnterDesignMode,
        DebuggerEnterBreakMode,
        DebuggerContextChanged,
    }

    public enum SensorCommandEvent
    {
        CommandBeforeExecute,
        CommandAfterExecute,
    }

    public enum SensorDocumentEvent
    {                                               
        DocumentClosing,
        DocumentSaved,
        DocumentOpened
    }
}
