namespace NActivitySensor
{
    #region Usings
    using NActivitySensor.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public interface IReporter
    {
        #region Methods
        void Report(Report reportModel);
        #endregion
    }
}
