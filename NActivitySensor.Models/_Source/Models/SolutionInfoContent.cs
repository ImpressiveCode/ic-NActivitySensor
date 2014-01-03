namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion
    
    public class SolutionInfoContent
    {
        #region Properties
        public string SolutionName
        {
            get;
            set;
        }

        public IEnumerable<string> Projects
        {
            get;
            set;
        }
        #endregion
    }
}
