namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public interface IConnectContext
    {
        object Application
        {
            get;
        }
    }
}
