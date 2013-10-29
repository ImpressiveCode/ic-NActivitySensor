using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using System.Diagnostics;

namespace NActivitySensor
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{
        #region Private variables
        private Logger _logger;
        private int _numberOfSecondsToSetInactive = 60;
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private SolutionEvents _solutionEvents;
        private BuildEvents _buildEvents;
        private ProjectsEvents _projectsEvents;
        private List<string> _projectsBuildReport;
        private bool IsAlive;
        System.Timers.Timer _timer;
        #endregion

        #region Solution events
        void solutionEvents_Opened()
        {
            _logger.Log("solutionEvents_Opened(): " + _applicationObject.Solution.FullName);
            TickAlive();
        }

        void solutionEvents_BeforeClosing()
        {
            _logger.Log("solutionEvents_BeforeClosing(): " + _applicationObject.Solution.FullName);
            TickAlive();
        }
        #endregion

        #region Build events
        void _buildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            _projectsBuildReport.Add(DateTime.Now.ToUniversalTime().ToString() + "  " + (Success ? "Succeeded" : "Failed   ") + " | " + Project + " [" + ProjectConfig + "|" + Platform + "]");
            _logger.Log("buildEvents_OnBuildProjConfigDone: " + DateTime.Now.ToUniversalTime().ToString() + "  " + (Success ? "Succeeded" : "Failed   ") + " | " + Project + " [" + ProjectConfig + "|" + Platform + "]");
            TickAlive();
        }

        void _buildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            _projectsBuildReport.Add("Build scope:" + Scope.ToString());
            _projectsBuildReport.Add("Build action:" + Action.ToString());
            TickAlive();
        }

        void buildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            _projectsBuildReport.Clear();
            _logger.Log("buildEvents_OnBuildBegin(): " + DateTime.Now.ToUniversalTime().ToString() + " (Scope: " + Scope.ToString() + ", Action: " + Action.ToString() + ")");
            TickAlive();
        }
        #endregion

        #region Public
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
            _logger = new Logger();
            _timer = new System.Timers.Timer(_numberOfSecondsToSetInactive * 1000);
            _projectsBuildReport = new List<string>();
            IsAlive = true;
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            _applicationObject.Events.DocumentEvents.DocumentClosing += DocumentEventsTick;
            _applicationObject.Events.DocumentEvents.DocumentSaved += DocumentEventsTick;
            _applicationObject.Events.DocumentEvents.DocumentOpened += DocumentEventsTick;


            // Add solution item events
            _solutionEvents = _applicationObject.Events.SolutionEvents;
            _solutionEvents.Opened += solutionEvents_Opened;
            _solutionEvents.BeforeClosing += solutionEvents_BeforeClosing;


            // Add build events
            _buildEvents = _applicationObject.Events.BuildEvents;
            _buildEvents.OnBuildBegin += buildEvents_OnBuildBegin;
            _buildEvents.OnBuildProjConfigDone += _buildEvents_OnBuildProjConfigDone;
            _buildEvents.OnBuildDone += _buildEvents_OnBuildDone;
        }

        void DocumentEventsTick(Document Document)
        {
            TickAlive();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsAlive)
            {
                _projectsBuildReport.Add(DateTime.Now.ToUniversalTime().ToString() + " User not active!");
                _logger.Log(DateTime.Now.ToUniversalTime().ToString() + " User not active!");
            }

            IsAlive = false;
        }


        private void TickAlive()
        {
            IsAlive = true;
        }


        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
        #endregion
    }
}