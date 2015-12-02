namespace NActivitySensor.Models
{
    #region Usings
    using System.Collections.Generic;

    #endregion

    public class SolutionInfoContent
    {
        #region Properties
        public string SolutionName
        {
            get;
            set;
        }

        public IEnumerable<ProjectInfoContent> Projects
        {
            get;
            set;
        }
        #endregion
    }
}
