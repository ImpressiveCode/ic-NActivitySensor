namespace NActivitySensor
{
    #region Usings
    using Autofac;
    using EnvDTE;
    using EnvDTE80;
    using Extensibility;
    using Microsoft.VisualStudio.CommandBars;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.Reflection;
    #endregion

    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDisposable
    {
        #region Private variables
        private Distributor _Distributor;
        private Bootstrapper _Bootstrapper;
        private ILogger _Logger;
        private readonly System.Configuration.Configuration _PluginConfiguration;
        private IConnectContext _ConnectContext;
        #endregion

        #region Public
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
            _PluginConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            _Logger = new FileLogger();
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#")]
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                _ConnectContext = new DefaultConnectContext(application, addInInst, _PluginConfiguration);
                _Logger = new Log4NetLogger(_ConnectContext);

                _Bootstrapper = new Bootstrapper(application, addInInst, _Logger, _ConnectContext);

                _Distributor = _Bootstrapper.Resolve<Distributor>();

                // Test - adding menubar
                // TODO: Create separate menu classes
                //var DTEApplication = (DTE2)application;
                //var CommandBars = (CommandBars)DTEApplication.CommandBars;
                //var MenuBar = CommandBars["MenuBar"];
                //var ToolsCommandBar = CommandBars["Tools"];
                //var HelpCommandBar = CommandBars["Help"];
                //CommandBar SensorCommandBar = (CommandBar)DTEApplication.Commands.AddCommandBar("NAcvititySensor", vsCommandBarType.vsCommandBarTypeMenu, MenuBar, 1);

                _Distributor.OnConnection(application, connectMode, addInInst, ref custom);
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#")]
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                _Distributor.OnDisconnection(disconnectMode, ref custom);
                Dispose();
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void OnAddInsUpdate(ref Array custom)
        {
            try
            {
                _Distributor.OnAddInsUpdate(ref custom);
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void OnStartupComplete(ref Array custom)
        {
            try
            {
                _Distributor.OnStartupComplete(ref custom);
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void OnBeginShutdown(ref Array custom)
        {
            try
            {
                _Distributor.OnBeginShutdown(ref custom);
            }
            catch (Exception exception)
            {
                _Logger.Log(exception);
            }
        }
        #endregion

        #region IDisposable methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Distributor != null)
                {
                    _Distributor.Dispose();
                    _Distributor = null;
                }

                if (_Bootstrapper != null)
                {
                    _Bootstrapper.Dispose();
                    _Bootstrapper = null;
                }
            }
        }
        #endregion
    }
}