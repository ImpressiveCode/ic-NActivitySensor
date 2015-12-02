namespace NActivitySensor
{
    #region Sensor events enums
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
        BuildProjConfigDone,
        BuildBegin,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
        BuildProjConfigBegin
    }

    public enum SensorUserEvent
    {
        UserInactive,
        UserActiveAgain
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WindowPane")]
        WindowPaneUpdated,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WindowPane")]
        WindowPaneClearing,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WindowPane")]
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
    #endregion
}
