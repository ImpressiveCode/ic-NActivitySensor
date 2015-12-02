using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Configuration;
using System.Reflection;

namespace NActivitySensor
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(NActivitySensorPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    public sealed class NActivitySensorPackage : Package
    {
        #region Private constants
        public const string PackageGuidString = "5f740d7d-d947-4025-ab69-5c7d0c2b8eb9";
        #endregion

        #region Private variables
        private Bootstrapper _Bootstrapper;
        private DefaultConnectContext _ConnectContext;
        private Distributor _Distributor;
        private ILogger _Logger;
        private Configuration _PluginConfiguration;
        #endregion

        #region Constructors
        public NActivitySensorPackage()
        {
        }
        #endregion

        #region Override methods
        protected override void Initialize()
        {
            base.Initialize();

            try
            {
                this.MyInitialize();
            }
            catch(Exception exception)
            {
                System.Windows.Forms.MessageBox.Show(exception.ToString());
            }
        }
        #endregion

        #region My methods
        private void MyInitialize()
        {
            _PluginConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            _Logger = new FileLogger();

            EnvDTE80.DTE2 dte2;
            dte2 = (EnvDTE80.DTE2)Marshal.GetActiveObject("VisualStudio.DTE.14.0");

            _ConnectContext = new DefaultConnectContext(dte2, _PluginConfiguration);
            _Logger = new Log4NetLogger(_ConnectContext);

            _Bootstrapper = new Bootstrapper(dte2, _Logger, _ConnectContext);

            _Distributor = _Bootstrapper.Resolve<Distributor>();

            _Distributor.OnConnection(dte2);
        }
        #endregion
    }
}
