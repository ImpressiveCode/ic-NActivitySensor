namespace NActivitySensor
{
    #region Usings
    using Autofac;
    using Extensibility;
    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDisposable
    {
        #region Private variables
        private Distributor _Distributor;
        private Bootstrapper _Bootstrapper;
        #endregion

        #region Public
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
            _Bootstrapper = new Bootstrapper();
            _Distributor = new Distributor(_Bootstrapper.Scope.Resolve<IEnumerable<IActivitySensor>>());
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#")]
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _Distributor.OnConnection(application, connectMode, addInInst, ref custom);
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#")]
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            _Distributor.OnDisconnection(disconnectMode, ref custom);
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
            _Distributor.OnAddInsUpdate(ref custom);
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            _Distributor.OnStartupComplete(ref custom);
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            _Distributor.OnBeginShutdown(ref custom);
        }
        #endregion

        #region IDisposable methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
                }
            }
        }
        #endregion
    }
}