namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class ProjectRenamedContent
    {
        #region Properties
        public ProjectInfoContent Project
        {
            get;
            set;
        }

        public string OldName
        {
            get;
            set;
        }
        #endregion
    }
}
